
document.addEventListener('DOMContentLoaded', function () {
    let branchSelectModal;
    let positionSelectModal ;

    let EducationStatusSelect = new TomSelect($("#EducationStatusSelect"))
    let MaritalStatusSelect = new TomSelect($("#MaritalStatusSelect"));
    let BodySizeSelect = new TomSelect($("#BodySizeSelect"));
    let BloodGroupSelect =   new TomSelect($("#BloodGroupSelect"));
   $.ajax({ //TODO
       type: "POST",
       url : "/get-select-items"
   }).done(function (res) {
           $('#branchSelectModal').empty();
           $.each(res.branches, function (index , branch) {
               $('#branchSelectModal').append(`<option value='${branch.id}'>${branch.name}</option>`);
           });
           $.each(res.positions, function (index , position) {
               $('#positionSelectModal').append(`<option value='${position.id}'>${position.name}</option>`);
           });
       branchSelectModal = new TomSelect($("#branchSelectModal"));
       positionSelectModal = new TomSelect($("#positionSelectModal"));
       positionSelectModal.clear();
       branchSelectModal.clear();
   });
   //Formu Resetleme
    $('#addModal').on('hidden.bs.modal', function (e) {
        // Form alanınızı resetleme
        $('#addPersonalForm')[0].reset();
        positionSelectModal.clear();
        branchSelectModal.clear();
        EducationStatusSelect.clear();
        MaritalStatusSelect.clear();
        BodySizeSelect.clear();
        BloodGroupSelect.clear();
        $("#previousButton").click();
    });
    //Devam Et Butonu 
    $("#nextButton").on('click',function () {
        $(this).addClass("d-none");
        $("#personalAddButton").removeClass("d-none");
        $("#previousButton").removeClass("d-none");
        $('[data-step1]').addClass('d-none');
        $('[data-step2]').removeClass('d-none');
    });
    //Geri Dön Butonu
    $("#previousButton").on('click',function () {
        $(this).addClass("d-none");
        $("#personalAddButton").addClass("d-none");
        $("#nextButton").removeClass("d-none");
        $('[data-step1]').removeClass('d-none');
        $('[data-step2]').addClass('d-none');
    });
    //Personeli Ekle Butonu
    $("#personalAddButton").on("click", function (e) {
        e.preventDefault();
        let formData = $("#addPersonalForm").serializeArray();
        formData.forEach(function (f) {
            if (f.value ==="on"){
                f.value = true;
            }
        });
        //Formun id'sini kullanarak formu gönder
        $.ajax({
            type: "POST",
            url: "/create-personal",
            data: formData // Form verilerini al
        }).done(function (res) {
                if (res.isSuccess){
                    console.log("Başarılı");
                }
                else{
                    console.log(res.err);
                }
        }).then(function () {
            window.location = "/personeller"
        });
            
    });
});