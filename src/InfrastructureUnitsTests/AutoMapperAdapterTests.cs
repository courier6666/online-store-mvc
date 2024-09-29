using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using AutoMapper;
using Shouldly;
using Store.Core.Infrastructure.Mappers;

namespace InfrastructureUnitsTests
{
    public class AutoMapperAdapterTests
    {
        public class Source
        {
            public Source()
            {
                
            }
            public Source(int A)
            {
                this.A = A;
            }
            public int A { get; set; }
        }

        public class Destination
        {
            public Destination()
            {
                
            }
            public Destination(int A)
            {
                this.A = A;
            }
            public int A { get; set; }
            public override bool Equals(object? obj)
            {
                return obj != null &&
                       obj is Destination &&
                       (obj as Destination).A.Equals(A);
            }
        }
        public class DestinationWithMoreProperties
        {
            public DestinationWithMoreProperties()
            {
                
            }
            public DestinationWithMoreProperties(int A, int B)
            {
                this.A = A;
                this.B = B;
            }
            public int A { get; set; }
            public int B { get; set; }
            public override bool Equals(object? obj)
            {
                return obj != null &&
                       obj is DestinationWithMoreProperties &&
                       (obj as DestinationWithMoreProperties).A.Equals(A) &&
                       (obj as DestinationWithMoreProperties).B.Equals(B);
            }
        }

        public class InnerSource
        {
            public InnerSource()
            {
                
            }
            public InnerSource(int C)
            {
                this.C = C;
            }

            public int C { get; set; }
        }

        public class OuterSource
        {
            public OuterSource()
            {
                
            }
            public OuterSource(InnerSource innerSource, int A, int B)
            {
                this.InnerSource = innerSource;
                this.A = A;
                this.B = B;
            }
            public InnerSource InnerSource { get; set; }
            public int A { get; set; }
            public int B { get; set; }
        }
        public class InnerDest
        {
            public InnerDest()
            {
                
            }
            public InnerDest(int C)
            {
                this.C = C;
            }

            public int C { get; set; }
            public override bool Equals(object? obj)
            {
                return obj != null && obj is InnerDest && (obj as InnerDest).C == this.C;
            }
        }

        public class OuterDest
        {
            public OuterDest()
            {
                
            }
            public OuterDest(InnerDest innerDest, int A, int B)
            {
                this.InnerDest = innerDest;
                this.A = A;
                this.B = B;
            }
            public InnerDest InnerDest { get; set; }
            public int A { get; set; }
            public int B { get; set; }
            public override bool Equals(object? obj)
            {
                return obj != null &&
                       obj is OuterDest &&
                       (obj as OuterDest).InnerDest.Equals(this.InnerDest) &&
                       (obj as OuterDest).A.Equals(A) &&
                       (obj as OuterDest).B.Equals(B);
            }
        }

        private AutoMapperAdapter adapter;
        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Source, Destination>();
                cfg.CreateMap<Source, DestinationWithMoreProperties>().
                    ForMember(dest => dest.B, opt => opt.Ignore());
                cfg.CreateMap<InnerSource, InnerDest>();
                cfg.CreateMap<OuterSource, OuterDest>().
                    ForMember(o => o.InnerDest, o => o.MapFrom(src => src.InnerSource));
            });

            config.AssertConfigurationIsValid();
            adapter = new AutoMapperAdapter(config.CreateMapper());
        }

        [Test]
        public void MustMap_WithoutException_MapAdapter()
        {
            //arrange
            Source source = new Source(1);

            
            //act & assert
            Assert.DoesNotThrow(() =>
            {
                var dest = adapter.Map<Source, Destination>(source);
            });
        }
        [Test]
        public void MapEnumerables_PropertyOfMappedMustBeEqual_MapAdapter()
        {
            IEnumerable<Source> enumSource = new[]
            {
                new Source(1),
                new Source(2),
                new Source(3),
                new Source(4),
                new Source(5),
            };

            IEnumerable<Destination> expected = new[]
            {
                new Destination(1),
                new Destination(2),
                new Destination(3),
                new Destination(4),
                new Destination(5),
            };

            //act
            var dest = adapter.MapEnumerable<Source, Destination>(enumSource);
            
            //assert
            dest.ShouldBe(expected);
        }

        [Test]
        public void MapToExisting_PropertyOfMappedMustBeEqual_MapAdapter()
        {
            //arrange
            Source source = new Source(1);

            Destination existing = new Destination(0);

            Destination expected = new Destination(1);

            //act
            adapter.MapToExisting<Source, Destination>(source, ref existing);

            //assert
            existing.ShouldBe(expected);
        }
        [Test]
        public void MustMapToCertainProperties_WithoutException_MapAdapter()
        {
            //arrange
            Source source = new Source(1);


            //act & assert
            Assert.DoesNotThrow(() =>
            {
                var dest = adapter.Map<Source, Destination>(source);
            });
        }
        [Test]
        public void MapEnumerablesToCertainProperties_PropertyOfMappedMustBeEqual_MapAdapter()
        {
            IEnumerable<Source> enumSource = new[]
            {
                new Source(1),
                new Source(2),
                new Source(3),
                new Source(4),
                new Source(5),
            };

            IEnumerable<DestinationWithMoreProperties> expected = new[]
            {
                new DestinationWithMoreProperties(1, 0),
                new DestinationWithMoreProperties(2, 0),
                new DestinationWithMoreProperties(3, 0),
                new DestinationWithMoreProperties(4, 0),
                new DestinationWithMoreProperties(5, 0),
            };

            //act
            var dest = adapter.MapEnumerable<Source, DestinationWithMoreProperties>(enumSource);

            //assert
            dest.ShouldBe(expected);
        }

        [Test]
        public void MapToExistingCertainProperties_PropertyOfMappedMustBeEqual_MapAdapter()
        {
            //arrange
            Source source = new Source(1);

            DestinationWithMoreProperties existing = new DestinationWithMoreProperties(0, 5);

            DestinationWithMoreProperties expected = new DestinationWithMoreProperties(1, 5);

            //act
            adapter.MapToExisting<Source, DestinationWithMoreProperties>(source, ref existing);

            //assert
            existing.ShouldBe(expected);
        }

        [Test]
        public void MapNestedEntities_PropertiesOfMappedMustBeEqual_MapAdapter()
        {
            //arrange
            OuterSource source = new OuterSource(new InnerSource(3), 1, 2);

            OuterDest expectedDest = new OuterDest(new InnerDest(3), 1, 2);

            //act
            var mappedDest = adapter.Map<OuterSource, OuterDest>(source);

            //assert
            mappedDest.ShouldBe(expectedDest);
        }
    }
}