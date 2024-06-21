// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#ingredientSelect').change(function () {
        addIngredientToList();
    });
    $('#tagSelect').change(function () {
        addTagToList();
    });
});

function addIngredientToList() {
    let listEnd = $('#ListIngredientsSelect');
    let ingredientFromSelect = $("#ingredientSelect option:selected");

    if ($('#ListIngredientsSelect input[type="hidden"][value="' + ingredientFromSelect.val() + '"]').length > 0) {
        alert(ingredientFromSelect.text() + ' já está na lista de ingredientes.');
        $('#ingredientSelect').selectpicker('deselectAll');
        return;
    }

    let idSelect = new Date().getTime();

    listEnd.append('<div id="' + idSelect + '" class="row mb-2">' +
        '<div class="col-4">' +
        '<input class="form-control text-center" name="Quantities" type="text"/>' +
        '</div>' +
        '<div class="col-5">' +
        '<input type="hidden" name="Ingredients" value="' + ingredientFromSelect.val() + '"/>' +
        '<input class="form-control text-center" type="text" value="' + ingredientFromSelect.text() + '" readonly/>' +
        '</div>' +
        '<div class="col-3">' +
        '<button class="form-control" type="button" onclick="removeFromList(' + idSelect + ')">❌</button>' +
        '</div>' +
        '</div>'
    );

    $('#ingredientSelect').selectpicker('deselectAll');
}

function addTagToList() {
    let listEnd = $('#ListTagsSelect');
    let tagFromSelect = $("#tagSelect option:selected");
    
    if ($('#ListTagsSelect input[type="hidden"][value="' + tagFromSelect.val() + '"]').length > 0) {
        alert(tagFromSelect.text() + ' já está na lista de tags.');
        $('#ingredientSelect').selectpicker('deselectAll');
        return;
    }

    let idSelect = new Date().getTime();

    listEnd.append('<div id="' + idSelect + '" class="row mb-2">' +
        '<div class="col-9">' +
        '<input type="hidden" name="Tags" value="' + tagFromSelect.val() + '"/>' +
        '<input class="form-control text-center" type="text" value="' + tagFromSelect.text() + '" readonly/>' +
        '</div>' +
        '<div class="col-3">' +
        '<button class="form-control" type="button" onclick="removeFromList(' + idSelect + ')">❌</button>' +
        '</div>' +
        '</div>'
    );

    $('#ingredientSelect').selectpicker('deselectAll');
}

function removeFromList(id) {
    $("#" + id).remove();
}