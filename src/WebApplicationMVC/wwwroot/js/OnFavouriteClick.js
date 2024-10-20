
//userId will be provided in backend
function setProductFavourite(productId, onSuccess) {
    $.ajax({
        type: "POST",
        url: `Products/SetFavourite/${productId}`,
        success: function (result) {
            onSuccess();
        },
        failure: function () {
            alert("Failed to add favourite product!");
        }
    });
}

function removeProductFavourite(productId, onSuccess) {
    $.ajax({
        type: "POST",
        url: `Products/SetUnfavourite/${productId}`,
        success: function (result) {
            onSuccess();
        },
        failure: function () {
            alert("Failed to remove favourite product!");
        }
    });
}

function onFavouriteClick(id) {
    let favField = document.getElementById(id);
    let icon = favField.querySelector("i");
    switch (favField.dataset.isfav) {
        case 'True':
            removeProductFavourite(id.replace('product', ''), function () {
                favField.dataset.isfav = 'False';
                icon.className = "fa-regular fa-star";
            });
            break;
        case 'False':
            setProductFavourite(id.replace('product', ''), function () {
                favField.dataset.isfav = 'True';
                icon.className = "fa-solid fa-star";
            });
            break;
        default:
            alert("Data attribute is not 'True' or 'False'.");
            break;
    }
    
}