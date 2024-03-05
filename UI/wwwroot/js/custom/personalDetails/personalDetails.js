
document.addEventListener('DOMContentLoaded', function () {
    let branchSelect = $('#branchSelectModal');
    let positionSelect= $('#positionSelectModal');
    let PersonalGroupSelect = $('#PersonalGroupSelect');
    let EducationStatusSelect= $('#EducationStatusSelect');
    let MaritalStatusSelect= $('#MaritalStatusSelect');
    let BodySizeSelect= $('#BodySizeSelect');
    let BloodGroupSelect= $('#BloodGroupSelect');
    $.ajax({ 
        type: "GET", 
        url: `/get-personel-detaylari${window.location.search}`
    }).done(function (res) {
        if (res.isSuccess) {
            $('#HeaderPersonalNameSurname').text(res.data.nameSurname);
            $('#HeaderPersonalBranchPosition').text((res.data.branches.find(p => p.id === res.data.branch_Id) || {}).name + " - " +(res.data.positions.find(p => p.id === res.data.position_Id).name));
            $('#personelİzinleriPage').attr('href', `/personel-izinleri?id=${res.data.id}`);
            if (res.data.status === 0) { // Online
                $('#headerButton').addClass("btn-secondary").removeClass("btn-orange");
                $('#headerButton span').html("İşten Çıkar");
            } else {
                $('#headerButton').addClass("btn-orange").removeClass("btn-secondary");
                $('#headerButton span').html("İşe Geri Al");
            }
            fillpersonalDetailsInputs(res.data);
        } else {
            $('#error-modal-message').text(res.message)
            $('#error-modal').modal('show')
            $('#error-modal-button').click(function () {
                window.location.href = "/Personal";
            })
        }
    });
    function fillpersonalDetailsInputs(data) {
        // Avatar ve diğer basit bilgilerin ayarlanması
        $('#personalAvatar').html(data.nameSurname.charAt(0));
        $('#badgeTotalTakenLeave').html(saatleriGunVeSaatlereCevir(data.totalTakenLeave));
        $('#badgeFoodAid').html(data.foodAid);
        $('#badgeTotalYearLeave').html(data.totalYearLeave);
        $('#badgeUsedYearLeave').html(data.usedYearLeave);
        $('#balanceYearLeave').html(data.totalYearLeave - data.usedYearLeave);
        
        kalanIzinKumulatif(data.usedYearLeave,new Date(data.startJobDate));
        function initializeFlatpickr(input) {
           return flatpickr(input, {
                altInput: true,
                altFormat: "d F Y",
                dateFormat: "Y-m-d",
                locale: {
                    weekdays: {
                        longhand: ['Pazar', 'Pazartesi','Salı','Çarşamba','Perşembe', 'Cuma','Cumartesi'],
                        shorthand: ['Paz', 'Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt']
                    },
                    months: {
                        longhand: ['Ocak','Şubat','Mart','Nisan','Mayıs','Haziran','Temmuz','Ağustos', 'Eylül','Ekim','Kasım','Aralık'],
                        shorthand: ['Oca','Şub','Mar','Nis','May','Haz','Tem','Ağu','Eyl','Eki','Kas','Ara']
                    },
                    today: 'Bugün',
                    clear: 'Temizle'
                }
            });
        }
        // Form elemanlarına değer atama işlemleri
        function setFormValues() {
            $('input[name="ID"]').val(data.id);
            $('input[name="PersonalDetails.ID"]').val(data.personalDetails.id);
            $('input[name="NameSurname"]').val(data.nameSurname);
            initializeFlatpickr($('input[name="BirthDate"]')).setDate(data.birthDate);
            $('input[name="PersonalDetails.BirthPlace"]').val( data.personalDetails.birthPlace);
            $('input[name="IdentificationNumber"]').val(data.identificationNumber);
            $('input[name="RegistirationNumber"]').val(data.registirationNumber);
            $('input[name="PersonalDetails.SskNumber"]').val(data.personalDetails.sskNumber);
            $('input[name="PersonalDetails.SgkCode"]').val(data.personalDetails.sgkCode);
            $('input[name="PersonalDetails.Salary"]').val(data.personalDetails.salary);
            initializeFlatpickr($('input[name="StartJobDate"]')).setDate(data.startJobDate); //TODO
            //initializeFlatpickr($('input[name="EndJobDate"]')).setDate(data.endJobDate); //TODO
            $('input[name="PersonalDetails.MotherName"]').val(data.personalDetails.motherName);
            $('input[name="PersonalDetails.FatherName"]').val(data.personalDetails.fatherName);
            $('input[name="Phonenumber"]').val(data.phonenumber);
            $('input[name="PersonalDetails.BankAccount"]').val(data.personalDetails.bankAccount);
            $('input[name="PersonalDetails.IBAN"]').val(data.personalDetails.iban);
            $('textarea[name="PersonalDetails.Address"]').val(data.personalDetails.address);
            $('[data-takenLeave]').val(parseInt(data.totalTakenLeave,10));
            
        }
        function saatleriGunVeSaatlereCevir(saat) {
            // Toplam saatleri gün ve saatlere çevir
            let gun = Math.floor(saat / 8);
            let kalanSaat = saat % 8;

            // Sonucu döndür
            return gun + " gün " + kalanSaat + " saat";
        }
        // Şube ve pozisyon seçeneklerinin ayarlanması
        function setSelects() {
            branchSelect.empty();
            positionSelect.empty();
            $.each(data.branches, function (index, branch) {
                branchSelect.append(`<option value='${branch.id}'>${branch.name}</option>`);
            });
            $.each(data.positions, function (index, position) {
                positionSelect.append(`<option value='${position.id}'>${position.name}</option>`);
            });

            branchSelect.val(data.branch_Id);
            positionSelect.val(data.position_Id);
            PersonalGroupSelect.val(data.personalDetails.personalGroup);
            EducationStatusSelect.val(data.personalDetails.educationStatus);
            MaritalStatusSelect.val(data.personalDetails.maritalStatus);
            BodySizeSelect.val(data.personalDetails.bodySize);
            BloodGroupSelect.val(data.personalDetails.bloodGroup);
        }
        function kalanIzinKumulatif(kullanilanYillikIzin,iseBaslamaTarihi) {
            let outputDiv = document.getElementById("balanceYearLeaveDetail");
            let calisilanYil = 0;
            let kalanGun = 0;
            
            for (let year = iseBaslamaTarihi.getFullYear();year <= new Date().getFullYear();year++){
                if (calisilanYil === 0){
                    outputDiv.innerHTML += "Yıl: " + year + ", Hak Edilen: " + kalanGun + "<br>";
                    calisilanYil++;
                    continue;
                }
                //TODO
                //if (yas >=50) {
                //    //do something and continue;
                //}
                if (calisilanYil <= 5) {
                    kalanGun = Math.max(14 - kullanilanYillikIzin,0);
                    kullanilanYillikIzin = Math.max(kullanilanYillikIzin - 14 , 0)
                }else if(calisilanYil < 15){
                    kalanGun = Math.max(20 - kullanilanYillikIzin,0);
                    kullanilanYillikIzin = Math.max(kullanilanYillikIzin - 20 , 0)
                }else{
                    kalanGun = Math.max(26 - kullanilanYillikIzin,0);
                    kullanilanYillikIzin = Math.max(kullanilanYillikIzin - 26 , 0) 
                }
                //TODO
                if (iseBaslamaTarihi) {
                    outputDiv.innerHTML += "Yıl: " + year + ", Hak Edilen: " + kalanGun + "<br>";
                }
                
                calisilanYil++;
            }
        }
        
        // Emeklilik ve engellilik durumlarının işaretlenmesi
        function setCheckboxes() {
            data.personalDetails.handicapped ? $('input[name="PersonalDetails.Handicapped"]').prop('checked', true) : "";
            if(data.retiredOrOld){
                let emeklilikLabel = $('label[for="RetiredOrOldInput"]');
                let emeklilikCheckBox = $('input[name="RetiredOrOld"]');
                emeklilikCheckBox.prop('checked', true)
                if($(emeklilikCheckBox).is(':checked')){
                    emeklilikLabel.addClass("required")
                    $(emeklilikCheckBox).closest('.d-flex.flex-column').append(`<input class="form-control m-2 flatpickr-input" data-required="true" type="date" name="RetiredDate" id="RetiredDateInput" placeholder="Emeklilik Tarihi" disabled>`);
                    initializeFlatpickr(document.getElementById('RetiredDateInput')).setDate(data.retiredDate);
                }
            }
        }

        // Cinsiyet seçiminin işaretlenmesi
        function setGenderSelection() {
            data.gender === "Erkek" ? $('input[value="Erkek"]').prop('checked', true) : $('input[value="Kadın"]').prop('checked', true);
        }
        
        
        setFormValues();
        setSelects();
        setCheckboxes();
        setGenderSelection();

        
    }
    function onClickEvents() {
        //Düzenlemeyi Etkinleştir Butonu Tıklandığında
        $('#enableEditButton').on('click',function () {
            let inputs =$('#updatePersonalForm input');
            inputs.prop('disabled', false);
            $('[data-hourInput]').prop('disabled',false);
            $('[data-addHour]').prop('disabled',false);
            $('[data-removeHour]').prop('disabled',false);
            
            $('#updatePersonalForm textarea').prop('disabled', false);
            $('#updatePersonalForm select').prop('disabled', false);
            new TomSelect(positionSelect); // Seçilen select'i alın
            new TomSelect(branchSelect); // Seçilen select'i alın
            new TomSelect(PersonalGroupSelect);
            new TomSelect(EducationStatusSelect); // Seçilen select'i alın
            new TomSelect(MaritalStatusSelect); // Seçilen select'i alın
            new TomSelect(BodySizeSelect); // Seçilen select'i alın
            new TomSelect(BloodGroupSelect); // Seçilen select'i alın
            $('#enableEditButton').prop('disabled', true);
            $('#updatePersonalButton').prop('disabled', false);
        });
        //Alacak İzin Saat Ekle tıklandığında çalışan metod
        $('[data-addHour]').on('click',function () {
            if($('[data-hourInput]').val() !== ''){
                $('[data-takenLeave]').val(function (index, currentValue) {
                    return parseInt(currentValue,10) + parseInt($('[data-hourInput]').val(),10);
                }); 
            }
            $('[data-hourInput]').val("");
        });
        //Alacak İzin Saat Çıkart tıklandığında çalışan metod
        $('[data-removeHour]').on('click',function () {
            if ($('[data-hourInput]').val() !== '') {
                $('[data-takenLeave]').val(function (index, currentValue) {
                    return parseInt(currentValue, 10) - parseInt($('[data-hourInput]').val(), 10);
                });
            } 
            $('[data-hourInput]').val("");
        });
        //Personeli Sil Butonu Tıklandığında Butonu Tıklandığında
        $("#deleteButton").on("click", function() {
            let personalName = $('input[name="NameSurname"]').val();
            $("#personalNamePlaceholder").text(personalName); // inputa taşıyın
        });
        //Modal üzerindeki Personeli Sil Butonu Tıklandığında
        $("#deleteSubmitBtn").on('click',function () {
            $.ajax({ 
                type: "POST",
                url: `/personel-sil?id=${$('input[name="ID"]').val()}`
            }).done(function (res) {
                if (res.isSuccess){
                    $('#success-modal-message').text("Personel Başarılı Bir Şekilde Silindi")
                    $('#success-modal').modal('show')
                    $('#success-modal-button').click(function () {
                        window.location.href = "/Personal";
                    })
                }else{
                    $('#error-modal-message').text(res.message)
                    $('#error-modal').modal('show')
                }
            }); 
        });
        //İşten Çıkar veya İşe Geri Al Butonu Tıklandığında
        $('#headerButton').on("click", function () {
           $.ajax({ 
               type: "POST",
               url: `/personel-durumu${window.location.search}`
           }).done(function (res) {
               if (res.isSuccess){
                  console.log("Başarılı"); 
               }else{
                   console.log("Başarısız");
               }
           }).then(function () {
               window.location = window.location;
           });
        });
        //Personeli Güncelle Butonu Tıklandığında
        $('#updatePersonalButton').on('click',function () {
            let formData = $("#updatePersonalForm").serializeArray();
            formData.forEach(function (f) {
                if (f.value ==="on"){
                    f.value = true;
                }
            });
            //Formun id'sini kullanarak formu gönder
            $.ajax({
                type: "POST",
                url: "/personel-detaylari",
                data: formData // Form verilerini al
            }).done(function (res) {
                if(res.isSuccess){
                    $('#success-modal-message').text("Personel Başarılı Bir Şekilde Güncellendi.")
                    $('#success-modal').modal('show');
                    $('#success-modal-button').click(function () {
                        window.location.reload();
                    })
                }else{
                    $('#error-modal-message').text(res.message)
                    $('#error-modal').modal('show');
                }
            });
        });
        //Emeklilik durumu aç kapa yapıldığında çalışan metod
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
    }
    onClickEvents();
});

