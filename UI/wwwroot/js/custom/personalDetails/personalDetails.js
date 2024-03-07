document.addEventListener('DOMContentLoaded', function () {
    let branchSelect = $('#branchSelectModal');
    let positionSelect = $('#positionSelectModal');
    let PersonalGroupSelect = $('#PersonalGroupSelect');
    let EducationStatusSelect = $('#EducationStatusSelect');
    let MaritalStatusSelect = $('#MaritalStatusSelect');
    let BodySizeSelect = $('#BodySizeSelect');
    let BloodGroupSelect = $('#BloodGroupSelect');
    $.ajax({
        type: "GET",
        url: `/get-personel-detaylari${window.location.search}`
    }).done(function (res) {
        if (res.isSuccess) {
            $('#HeaderPersonalNameSurname').text(res.data.nameSurname);
            $('#HeaderPersonalBranchPosition').text((res.data.branches.find(p => p.id === res.data.branch_Id) || {}).name + " - " + (res.data.positions.find(p => p.id === res.data.position_Id).name));
            $('#personelİzinleriPage').attr('href', `/personel-izinleri?id=${res.data.id}`);
            if (res.data.status === 0) { // Online
                $('#istenCikarButton').addClass("btn-secondary").removeClass("btn-orange");
                $('#istenCikarButton span').html("İşten Çıkar");
            } else {
                $('#istenCikarButton').addClass("btn-orange").removeClass("btn-secondary");
                $('#istenCikarButton span').html("İşe Geri Al");
            }
            fillpersonalDetailsInputs(res.data);
            onClickEvents(res.data);
        } else {
            $('#error-modal-message').text(res.message)
            $('#error-modal').modal('show')
            $('#error-modal-button').click(function () {
                window.location.href = "/Personal";
            })
        }
    });

    function fillpersonalDetailsInputs(data) {
        //Personel İşten Çıkarılmış Personel ise
        if (data.status === 1) {
            $('#istenCikarilmaTarihiDiv').html(`
                <button id="CikisTarihiShow" type="button" class="btn">
                    <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-calendar-minus" width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                        <path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M12.5 21h-6.5a2 2 0 0 1 -2 -2v-12a2 2 0 0 1 2 -2h12a2 2 0 0 1 2 2v8"/><path d="M16 3v4"/><path d="M8 3v4"/><path d="M4 11h16"/><path d="M16 19h6"/>
                    </svg>
                    <span></span>
                </button>
            `)
            let formattedDate = new Date(data.endJobDate).toLocaleDateString("tr-TR", {
                day: 'numeric',
                month: 'long',
                year: 'numeric'
            });
            $('#CikisTarihiShow span').html("İşten Çıkış Tarihi: " + formattedDate)
        }
        // Avatar ve diğer basit bilgilerin ayarlanması
        $('#personalAvatar').html(data.nameSurname.charAt(0));
        $('#badgeTotalTakenLeave').html(saatleriGunVeSaatlereCevir(data.totalTakenLeave));
        $('#badgeFoodAid').html(data.foodAid);
        $('#badgeTotalYearLeave').html(data.totalYearLeave);
        $('#badgeUsedYearLeave').html(data.usedYearLeave);
        $('#balanceYearLeave').html(data.totalYearLeave - data.usedYearLeave);
        kalanIzinKumulatif(data.usedYearLeave, new Date(data.startJobDate), new Date(data.birthDate), data.retiredOrOld, new Date(data.retiredDate));

        //Date Başlatıcı Fonksiyon
        function initializeFlatpickr(input) {
            return flatpickr(input, {
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

        // Form elemanlarına değer atama işlemleri
        function setFormValues() {
            $('input[name="ID"]').val(data.id);
            $('input[name="PersonalDetails.ID"]').val(data.personalDetails.id);
            $('input[name="NameSurname"]').val(data.nameSurname);
            initializeFlatpickr($('input[name="BirthDate"]')).setDate(data.birthDate);
            $('input[name="PersonalDetails.BirthPlace"]').val(data.personalDetails.birthPlace);
            $('input[name="IdentificationNumber"]').val(data.identificationNumber);
            $('input[name="RegistirationNumber"]').val(data.registirationNumber);
            $('input[name="PersonalDetails.SskNumber"]').val(data.personalDetails.sskNumber);
            $('input[name="PersonalDetails.SgkCode"]').val(data.personalDetails.sgkCode);
            $('input[name="PersonalDetails.Salary"]').val(data.personalDetails.salary);
            initializeFlatpickr($('#StartJobDateInput')).setDate(data.startJobDate); //TODO
            $('input[name="PersonalDetails.MotherName"]').val(data.personalDetails.motherName);
            $('input[name="PersonalDetails.FatherName"]').val(data.personalDetails.fatherName);
            $('input[name="Phonenumber"]').val(data.phonenumber);
            $('input[name="PersonalDetails.BankAccount"]').val(data.personalDetails.bankAccount);
            $('input[name="PersonalDetails.IBAN"]').val(data.personalDetails.iban);
            $('textarea[name="PersonalDetails.Address"]').val(data.personalDetails.address);
            //Manuel Ayarlar
            $('[data-takenLeave]').val(parseInt(data.totalTakenLeave, 10)); // Manuel Alacak İzin alanı
            $('[data-usedYearLeave]').val(parseInt(data.usedYearLeave, 10))
            $('[data-foodAid]').val(parseInt(data.foodAid, 10))
            initializeFlatpickr($("[data-foodAidDate]")).setDate(data.foodAidDate); //TODO
            if(data.foodAidDate === data.startJobDate){
                $('#foodAidIsDefault').text("Gıda Yardım Tarihi Varsayılan olarak İşe Başlangıç Tarihinden Alındı")
            }
            else{
                $('#foodAidIsDefault').text("Dipnot: Gıda Yardım Tarihi İşe Başlangıç Tarihiden Farklı") 
            }
            //Manuel Ayarlar
        }

        //Alacak İzin Saat bazından güne cevirme metodu
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

        function kalanIzinKumulatif(kullanilanYillikIzin, iseBaslamaTarihi, dogumTarihi, emeklilikDurumu, emeklilikTarihi) {
            let outputDiv = document.getElementById("balanceYearLeaveDetail");
            let calisilanYil = 0;
            let kalanGun = 0;
            let todayDate = new Date()
            for (let year = iseBaslamaTarihi.getFullYear(); year <= new Date().getFullYear(); year++) {
                // let pointerYearDate = new Date(todayDate)
                // pointerYearDate.setFullYear(year)
                let iseBaslamaKontrolDate = new Date(year, iseBaslamaTarihi.getMonth(), iseBaslamaTarihi.getDate())
                let calisanYas = calculateAge(iseBaslamaKontrolDate, dogumTarihi)
                if (calisilanYil === 0 && iseBaslamaTarihi <= todayDate) {
                    outputDiv.innerHTML += "Yıl: " + year + ", Hak Edilen: " + kalanGun + "<br>";
                    calisilanYil++;
                    continue;
                }
                

                if (calisanYas >= 50 || calisanYas < 18 || (emeklilikDurumu && iseBaslamaKontrolDate > emeklilikTarihi)) {
                    kalanGun = Math.max(20 - kullanilanYillikIzin, 0);
                    kullanilanYillikIzin = Math.max(kullanilanYillikIzin - 20, 0)
                } else {
                    if (calisilanYil <= 5) {
                        kalanGun = Math.max(14 - kullanilanYillikIzin, 0);
                        kullanilanYillikIzin = Math.max(kullanilanYillikIzin - 14, 0)
                    } else if (calisilanYil < 15) {
                        kalanGun = Math.max(20 - kullanilanYillikIzin, 0);
                        kullanilanYillikIzin = Math.max(kullanilanYillikIzin - 20, 0)
                    } else {
                        kalanGun = Math.max(26 - kullanilanYillikIzin, 0);
                        kullanilanYillikIzin = Math.max(kullanilanYillikIzin - 26, 0)
                    }
                }
                

                if (year === todayDate.getFullYear() && iseBaslamaKontrolDate > todayDate) {
                    outputDiv.innerHTML += "Yıl: " + year + ", Hak Edilen: " + "Bekliyor" + "<br>";
                } else {
                    if (iseBaslamaTarihi) {
                        outputDiv.innerHTML += "Yıl: " + year + ", Hak Edilen: " + kalanGun + "<br>";
                    }
                }


                calisilanYil++;

                function calculateAge(pointerYearDate, dogumTarihi) {
                    let age = pointerYearDate.getFullYear() - dogumTarihi.getFullYear();
                    if (pointerYearDate.getMonth() < dogumTarihi.getMonth()) {
                        age--;
                    } else if (pointerYearDate.getMonth() === dogumTarihi.getMonth() && pointerYearDate.getDate() < dogumTarihi.getDate()) {
                        age--;
                    }
                    return age;
                }

            }
        }

        // Emeklilik ve engellilik durumlarının işaretlenmesi
        function setCheckboxes() {
            data.personalDetails.handicapped ? $('input[name="PersonalDetails.Handicapped"]').prop('checked', true) : "";
            if (data.retiredOrOld) {
                let emeklilikLabel = $('label[for="RetiredOrOldInput"]');
                let emeklilikCheckBox = $('input[name="RetiredOrOld"]');
                emeklilikCheckBox.prop('checked', true)
                if ($(emeklilikCheckBox).is(':checked')) {
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
    //Validasyon Fonksiyonu
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
    function onClickEvents(data) {
        //Düzenlemeyi Etkinleştir Butonu Tıklandığında
        $('#enableEditButton').on('click', function () {
            let inputs = $('#updatePersonalForm input');
            inputs.prop('disabled', false);
            $('[data-hourInput]').prop('disabled', false);
            $('[data-addHour]').prop('disabled', false);
            $('[data-removeHour]').prop('disabled', false);

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
        $('[data-addHour]').on('click', function () {
            if ($('[data-hourInput]').val() !== '') {
                $('[data-takenLeave]').val(function (index, currentValue) {
                    return parseInt(currentValue, 10) + parseInt($('[data-hourInput]').val(), 10);
                });
            }
            $('[data-hourInput]').val("");
        });
        //Alacak İzin Saat Çıkart tıklandığında çalışan metod
        $('[data-removeHour]').on('click', function () {
            if ($('[data-hourInput]').val() !== '') {
                $('[data-takenLeave]').val(function (index, currentValue) {
                    return parseInt(currentValue, 10) - parseInt($('[data-hourInput]').val(), 10);
                });
            }
            $('[data-hourInput]').val("");
        });
        //Maaş İnputu Regex Kontrolu
        $('input[name="PersonalDetails.Salary"]').keypress(function (event) {
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
        //Emeklilik durumu aç kapa yapıldığında çalışan metod
        $('#RetiredOrOldInput').change(function () {
            let emeklilikLabel = $('label[for="RetiredOrOldInput"]');
            if ($(this).is(':checked')) {
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
            } else {
                emeklilikLabel.removeClass("required")
                let retiredDateInput = document.getElementById('RetiredDateInput');
                let fpInstance = flatpickr(retiredDateInput);
                fpInstance.destroy();
                retiredDateInput.remove();
            }
        });
        //Personeli Güncelle Butonu Tıklandığında
        $('#updatePersonalButton').on('click', function () {
            let formData = $("#updatePersonalForm").serializeArray();
            let usedYearLeaveValue = formData.find(item => item.name === "UsedYearLeave").value
            let foodAidValue = formData.find(item => item.name === "FoodAid").value
            if (!checkRequiredFields()) {
                $('#error-modal-message').text("Lütfen Zorunlu Alanları Girdiğinizden Emin Olunuz.")
                $('#error-modal').modal('show');
                return; // Fonksiyondan çık
            }
            if (usedYearLeaveValue > data.totalYearLeave) {
                $('#error-modal-message').text("Kullanılan Yıllık İzin Hak edilenden büyük olamaz.")
                $('#error-modal').modal('show');
                return true;
            }
            if (usedYearLeaveValue < 0) {
                $('#error-modal-message').text("Kullanılan Yıllık İzin 0 dan küçük olamaz.")
                $('#error-modal').modal('show');
                return true;
            }
            if (foodAidValue < 0) {
                $('#error-modal-message').text("Gıda Yardımı 0 dan küçük olamaz.")
                $('#error-modal').modal('show');
                return true;
            }
            formData.forEach(function (f) {
                if (f.value === "on") {
                    f.value = true;
                }
            });
            //Formun id'sini kullanarak formu gönder
            $.ajax({
                type: "POST",
                url: "/personel-detaylari",
                data: formData // Form verilerini al
            }).done(function (res) {
                if (res.isSuccess) {
                    $('#success-modal-message').text("Personel Başarılı Bir Şekilde Güncellendi.")
                    $('#success-modal').modal('show');
                    $('#success-modal-button').click(function () {
                        window.location.reload();
                    })
                } else {
                    $('#error-modal-message').text(res.message)
                    $('#error-modal').modal('show');
                }
            });
        });

        //Personeli Sil Butonu Tıklandığında Butonu Tıklandığında
        $("#deleteButton").on("click", function () {
            let personalName = $('input[name="NameSurname"]').val();
            $("#personalNamePlaceholder").text(personalName); // inputa taşıyın
        });
        //Modal üzerindeki Personeli Sil Butonu Tıklandığında
        $("#deleteSubmitBtn").on('click', function () {
            $.ajax({
                type: "POST",
                url: `/personel-sil?id=${$('input[name="ID"]').val()}`
            }).done(function (res) {
                if (res.isSuccess) {
                    $('#success-modal-message').text("Personel Başarılı Bir Şekilde Silindi")
                    $('#success-modal').modal('show')
                    $('#success-modal-button').click(function () {
                        window.location.href = "/Personal";
                    })
                } else {
                    $('#error-modal-message').text(res.message)
                    $('#error-modal').modal('show')
                }
            });
        });
        //Personeli İşten Çıkar veya İşe Geri Al Butonu Tıklandığında 
        $("#istenCikarButton").on("click", function () {
            if (data.status === 0) {
                $('#istenCikarModal').modal('show')
                let personalName = $('input[name="NameSurname"]').val();
                $("#istenCikarPersonelAd").text(personalName); // inputa taşıyın
                let istenCikarCikisTarihi = $("#istenCikarForm input[name='EndJobDate']")
                flatpickr(istenCikarCikisTarihi, {
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
            if (data.status === 1) {
                $('#iseGeriAlModal').modal('show')
                let personalName = $('input[name="NameSurname"]').val();
                $("#iseGeriAlPersonelAd").text(personalName);
                let iseGeriAlIseGirisTarihi = $("#StartJobDateModalInput")
                flatpickr(iseGeriAlIseGirisTarihi, {
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
                let gidaYardimTarihi = $("#FoodAidDateModalInput")
                flatpickr(gidaYardimTarihi, {
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

        });
        //İşten Çıkar MODAL Butonu Tıklandığında
        $('#istenCikarSubmitButton').on("click", function () {
            let formData = $("#istenCikarForm").serializeArray();
            let hasEmptyField = false;
            $("#istenCikarForm input[name='ID'], #istenCikarForm input[name='EndJobDate']").each(function () {
                if ($(this).val().trim() === "") {
                    hasEmptyField = true;
                    return false; // Döngüyü sonlandır
                }
            });
            if (hasEmptyField) {
                $('#error-modal-message').text("Lütfen Çıkış Tarihini Seçtiğinizden Emin Olunuz!")
                $('#error-modal').modal('show');
            } else {
                $.ajax({
                    type: "POST",
                    url: `/personel-durumu`,
                    data: formData
                }).done(function (res) {
                    if (res.isSuccess) {
                        $('#success-modal-message').text("Personel Başarılı Bir Şekilde İşten Çıkarıldı.")
                        $('#success-modal').modal('show')
                        $('#success-modal-button').click(function () {
                            window.location.reload();
                        });
                    } else {
                        $('#error-modal-message').text(res.message)
                        $('#error-modal').modal('show');
                    }
                })
            }

        });
        //İşten Çıkarma Modal Resetleme
        $('#istenCikarModal').on('hidden.bs.modal', function (e) {
            let istenCikarCikisTarihi = $("#istenCikarForm input[name='EndJobDate']")
            // Form alanınızı resetleme
            $('#istenCikarForm')[0].reset();
            istenCikarCikisTarihi.val("")
            flatpickr(istenCikarCikisTarihi).clear()
        });
        //İşe Geri Al MODAL Butonu Tıklandığında
        $('#iseGeriAlSubmitButton').on("click", function () {
            let formData = $("#iseGeriAlForm").serializeArray();
            formData.forEach(function (f) {
                if (f.value === "on") {
                    f.value = true;
                }
            });
            let hasEmptyField = false;
            $("#iseGeriAlForm input[name='ID'], #iseGeriAlForm input[name='StartJobDate']").each(function () {
                if ($(this).val().trim() === "") {
                    hasEmptyField = true;
                    return false; // Döngüyü sonlandır
                }
            });
            if (hasEmptyField) {
                $('#error-modal-message').text("Lütfen İşe Giriş Tarihini Seçtiğinizden Emin Olunuz!")
                $('#error-modal').modal('show');
            } else {
                $.ajax({
                    type: "POST",
                    url: `/personel-durumu`,
                    data: formData
                }).done(function (res) {
                    if (res.isSuccess) {
                        $('#success-modal-message').text("Personel Başarılı Bir Şekilde Geri İşe Alındı.")
                        $('#success-modal').modal('show')
                        $('#success-modal-button').click(function () {
                            window.location.reload();
                        });
                    } else {
                        $('#error-modal-message').text(res.message)
                        $('#error-modal').modal('show');
                    }
                })
            }

        });
        //İşe Geri Al Modal Resetleme
        $('#iseGeriAlModal').on('hidden.bs.modal', function (e) {
            let iseGirisTarihi = $("#StartJobDateModalInput")
            let gidaYardimTarihi = $("#FoodAidDateModalInput")
            $('#iseGeriAlForm')[0].reset();
            iseGirisTarihi.val("")
            gidaYardimTarihi.val("")
            flatpickr(iseGirisTarihi).clear()
            flatpickr(gidaYardimTarihi).clear()
        });
    }

});

