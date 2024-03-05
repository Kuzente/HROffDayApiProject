
document.addEventListener('DOMContentLoaded', function () {
    
    let branchSelectModal;
    let positionSelectModal ;
    let PersonalGroupSelect = new TomSelect($("#PersonalGroupSelect"))
    let EducationStatusSelect = new TomSelect($("#EducationStatusSelect"))
    let MaritalStatusSelect = new TomSelect($("#MaritalStatusSelect"));
    let BodySizeSelect = new TomSelect($("#BodySizeSelect"));
    let BloodGroupSelect =   new TomSelect($("#BloodGroupSelect"));
   $.ajax({ //TODO
       type: "GET",
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
   //Zorunlu Alanların Validasyon fonksiyonu
    function checkRequiredFields() {
        let isValid = true;
        $("[data-required='true']").each(function() {
            if ($(this).val() === "") {
                isValid = false;
                return false;
            }
        });
        return isValid;
    }
    $('#RetiredOrOldInput').change(function () {
        let emeklilikLabel = $('label[for="RetiredOrOldInput"]');
       if($(this).is(':checked')){
           emeklilikLabel.addClass("required")
           $(this).closest('.d-flex.flex-column').append(`<input class="form-control m-2 flatpickr-input" data-required="true" type="date" name="RetiredDate" id="RetiredDateInput" placeholder="Emeklilik Tarihi">`);
           flatpickr(document.getElementById('RetiredDateInput'), {
               altInput: true,
               altFormat: "d F Y",
               dateFormat: "Y-m-d",
               locale: {
                   weekdays: {
                       longhand: ['Pazar', 'Pazartesi', 'Salı', 'Çarşamba', 'Perşembe', 'Cuma', 'Cumartesi'],
                       shorthand: ['Paz', 'Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt']
                   },
                   months: {
                       longhand: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'],
                       shorthand: ['Oca', 'Şub', 'Mar', 'Nis', 'May', 'Haz', 'Tem', 'Ağu', 'Eyl', 'Eki', 'Kas', 'Ara']
                   },
                   today: 'Bugün',
                   clear: 'Temizle'
               }
           });
       } 
       else{
           emeklilikLabel.removeClass("required")
           let retiredDateInput = document.getElementById('RetiredDateInput');
           let fpInstance = flatpickr(retiredDateInput);
           fpInstance.destroy();
           retiredDateInput.remove();
       }
    });
    //Maaş İnputu Regex Kontrolu
    $('input[name="PersonalDetails.Salary"]').keypress(function(event) {
        let charCode = (event.which) ? event.which : event.keyCode;
        let inputValue = $(this).val();
        // Regex ile kontrol et
        if (!/^\d*\.?\d*$/.test(inputValue + String.fromCharCode(charCode))) {
            event.preventDefault();
        }
        // En fazla bir nokta karakterine izin ver
        else if (inputValue.indexOf('.') !== -1 && charCode === 46) {
            event.preventDefault();
        }
    });
    //Formu Resetleme
    $('#addModal').on('hidden.bs.modal', function (e) {
        // Form alanınızı resetleme
        $('#addPersonalForm')[0].reset();
        positionSelectModal.clear();
        branchSelectModal.clear();
        PersonalGroupSelect.clear();
        EducationStatusSelect.clear();
        MaritalStatusSelect.clear();
        BodySizeSelect.clear();
        BloodGroupSelect.clear();
        $("#previousButton").click();
    });
    //Devam Et Butonu 
    $("#nextButton").on('click',function () {
        if (!checkRequiredFields()) {
            $('#error-modal-message').text("Lütfen Zorunlu Alanları Girdiğinizden Emin Olunuz.")
            $('#error-modal').modal('show');
            return; // Fonksiyondan çık
        }
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
               console.log(res)
               if (res.isSuccess){
                   $('#success-modal-message').text("Personel Başarılı Bir Şekilde Eklendi.")
                   $('#success-modal').modal('show')
                   $('#success-modal-button').click(function () {
                       window.location.reload();
                   });
               }
               else{
                   $('#error-modal-message').text(res.message)
                   $('#error-modal').modal('show')
               }
           })
        
            
    });
});