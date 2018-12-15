function alertErrorMessage(errorMessage) {
    $('#_ErrorFlashMessage').append("<div class=\"alert alert-danger\">\n                                    <a href=\"#\" class=\"close\" data-dismiss=\"alert\">&times;</a>\n                                    <strong>Error!</strong> " + errorMessage + "\n                                </div>");
}
function alertSuccessMessage(sucessMessage) {
    $('#_ErrorFlashMessage').append(" <div class=\"alert alert-success\">\n                                    <a href=\"#\" class=\"close\" data-dismiss=\"alert\">&times;</a>\n                                    <strong>Sucess!</strong> " + sucessMessage + "\n                                </div>");
}
//# sourceMappingURL=tool.js.map