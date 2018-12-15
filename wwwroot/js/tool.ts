function alertErrorMessage(errorMessage :string){
    $('#_ErrorFlashMessage').append(`<div class="alert alert-danger">
                                    <a href="#" class="close" data-dismiss="alert">&times;</a>
                                    <strong>Error!</strong> ${errorMessage}
                                </div>`)

}
function alertSuccessMessage(sucessMessage :string){
    $('#_ErrorFlashMessage').append(` <div class="alert alert-success">
                                    <a href="#" class="close" data-dismiss="alert">&times;</a>
                                    <strong>Sucess!</strong> ${sucessMessage}
                                </div>`)

}