
document.addEventListener('DOMContentLoaded', function () {
    const addPersonalForm = document.querySelector('#addPersonalForm');
    let validateAddPersonal = new JustValidate('#addPersonalForm',
            {
                submitFormAutomatically: true,
                validateBeforeSubmitting: true,
            }
    );
    validateAddPersonal
        .addField("#createNameSurname",[
                {
                    rule: 'required',
                    errorMessage: 'Boş Bırakılamaz!',
                },
                {
                    rule: 'maxLength',
                    value: 50,
                    errorMessage: '50 Karakterden Fazla Olamaz!'
                },
            ],
            {
                errorsContainer: addPersonalForm.querySelector('.errorContainer-nameSurname'),
            }
        )
        .addField(addPersonalForm.querySelector('input[name="StartJobDate"]'),[
                {
                    rule: 'required',
                    errorMessage: 'Boş Bırakılamaz!',
                },
            ],
            {
                errorsContainer: addPersonalForm.querySelector('.errorContainer-startJobDate'),
            }
        )
        .addField(addPersonalForm.querySelector('input[name="PersonalDetails.BirthPlace"]'),[
                {
                    rule: 'required',
                    errorMessage: 'Boş Bırakılamaz!',
                },
                {
                    rule: 'maxLength',
                    value: 50,
                    errorMessage: '50 Karakterden Fazla Olamaz!'
                },
            ],
            {
                errorsContainer: addPersonalForm.querySelector('.errorContainer-birthPlace'),
            }
        )
        .addField(addPersonalForm.querySelector('input[name="IdentificationNumber"]'),[
                {
                    rule: 'required',
                    errorMessage: 'Boş Bırakılamaz!',
                },
                {
                    rule: 'maxLength',
                    value: 50,
                    errorMessage: '50 Karakterden Fazla Olamaz!'
                },
            ],
            {
                errorsContainer: addPersonalForm.querySelector('.errorContainer-identificationNumber'),
            }
        )
        .addField(addPersonalForm.querySelector('input[name="RegistirationNumber"]'),[
                {
                    rule: 'required',
                    errorMessage: 'Boş Bırakılamaz!',
                },
                {
                    rule: 'maxLength',
                    value: 50,
                    errorMessage: '50 Karakterden Fazla Olamaz!'
                },
            ],
            {
                errorsContainer: addPersonalForm.querySelector('.errorContainer-registirationNumber'),
            }
        )
        .addField(addPersonalForm.querySelector('input[name="PersonalDetails.SskNumber"]'),[
                {
                    rule: 'required',
                    errorMessage: 'Boş Bırakılamaz!',
                },
                {
                    rule: 'maxLength',
                    value: 50,
                    errorMessage: '50 Karakterden Fazla Olamaz!'
                },
            ],
            {
                errorsContainer: addPersonalForm.querySelector('.errorContainer-sskNumber'),
            }
        )
        .addField(addPersonalForm.querySelector('input[name="PersonalDetails.SgkCode"]'),[
                {
                    rule: 'required',
                    errorMessage: 'Boş Bırakılamaz!',
                },
                {
                    rule: 'maxLength',
                    value: 50,
                    errorMessage: '50 Karakterden Fazla Olamaz!'
                },
            ],
            {
                errorsContainer: addPersonalForm.querySelector('.errorContainer-sgkCode'),
            }
        )
        .addField(addPersonalForm.querySelector('input[name="PersonalDetails.Salary"]'),[
                {
                    rule: 'required',
                    errorMessage: 'Boş Bırakılamaz!',
                },
                {
                    rule: 'maxLength',
                    value: 50,
                    errorMessage: '50 Karakterden Fazla Olamaz!'
                },
            ],
            {
                errorsContainer: addPersonalForm.querySelector('.errorContainer-salary'),
            }
        );
    let branchSelectModal;
    let positionSelectModal ;

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
        if(!validateAddPersonal.isValid){
            $("#personalAddButton").trigger('click');
            return;
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
               if (res.isSuccess){
                   window.location = "/personeller"
               }
               else{
                   //window.location.reload();
               }
           })
           //     .then(function () {
           //     window.location = "/personeller"
           // });
            
    });
});