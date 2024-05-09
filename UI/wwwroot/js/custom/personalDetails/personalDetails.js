document.addEventListener('DOMContentLoaded', function () {
    let branchSelect = $('#branchSelectModal');
    let positionSelect = $('#positionSelectModal');
    let departmantSelect = $('#departmantNameSelect');
    let PersonalGroupSelect = $('#PersonalGroupSelect');
    let EducationStatusSelect = $('#EducationStatusSelect');
    let MaritalStatusSelect = $('#MaritalStatusSelect');
    let BodySizeSelect = $('#BodySizeSelect');
    let BloodGroupSelect = $('#BloodGroupSelect');
    const updateButton = $('button[data-updateBtn]')
    const enableEditButton = $('button[data-enableEditBtn]')
    $.ajax({
        type: "GET",
        url: `/get-personel-detaylari${window.location.search}`
    }).done(function (res) {
        if (res.isSuccess) {
            $('#HeaderPersonalNameSurname').text(res.data.nameSurname);
            $('#HeaderPersonalBranchPosition').text((res.data.branches.find(p => p.id === res.data.branch_Id) || {}).name + " - " + (res.data.positions.find(p => p.id === res.data.position_Id).name));
            $('#personelİzinleriPage').attr('href', `/personel-izinleri?id=${res.data.id}`);
            $('#personelEksikGunPage').attr('href', `/personel-eksik-gun-listesi?id=${res.data.id}`);
            $('#personelNakilPage').attr('href', `/personel-nakil-listesi?id=${res.data.id}`);
            if (res.data.status === 0) { // Online
                $('#istenCikarButton').addClass("btn-secondary").removeClass("btn-orange");
                $('#istenCikarButton span').html("İşten Çıkar");
            } else {
                $('#istenCikarilmaTarihiDiv').html(`
                <button id="CikisTarihiShow" type="button" class="btn">
                    <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-calendar-minus" width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                        <path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M12.5 21h-6.5a2 2 0 0 1 -2 -2v-12a2 2 0 0 1 2 -2h12a2 2 0 0 1 2 2v8"/><path d="M16 3v4"/><path d="M8 3v4"/><path d="M4 11h16"/><path d="M16 19h6"/>
                    </svg>
                    <span></span>
                </button>
            `)
                let formattedDate = new Date(res.data.endJobDate).toLocaleDateString("tr-TR", {
                    day: 'numeric',
                    month: 'long',
                    year: 'numeric'
                });
                $('#CikisTarihiShow span').html("İşten Çıkış Tarihi: " + formattedDate)
                if(!res.data.isBackToWork){
                    $('#istenCikarButton').addClass("btn-orange").removeClass("btn-secondary");
                    $('#istenCikarButton span').html("İşe Geri Al"); 
                }
                else{
                    $('#istenCikarButton').remove()
                    
                }
            }
            fillpersonalDetailsInputs(res.data);
            onClickEvents(res.data);
            console.log(res.data)
            fillCumulativeTable(res.data.personalCumulatives,new Date(res.data.yearLeaveDate),res.data.status === 0)
            kalanIzinKumulatif(res.data.personalCumulatives , res.data.status === 0);
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
        let kalanIzinSayisiResponse = data.totalYearLeave - data.usedYearLeave
        $('#balanceYearLeave').html(kalanIzinSayisiResponse);
        

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
            $('input[name="Phonenumber"]').val(formatPhoneNumber(data.phonenumber));
            $('input[name="PersonalDetails.BankAccount"]').val(data.personalDetails.bankAccount);
            $('input[name="PersonalDetails.IBAN"]').val(formatIBAN(data.personalDetails.iban));
            $('textarea[name="PersonalDetails.Address"]').val(data.personalDetails.address);
            //Manuel Ayarlar
            $('[data-takenLeave]').val(data.totalTakenLeave); // Manuel Alacak İzin alanı
            $('[data-yearLeaveDate]').text(new Date(data.yearLeaveDate).toLocaleDateString('tr-TR', { year: 'numeric', month: 'long', day: 'numeric' }))
            if (!data.isYearLeaveRetired){
                $('#isYearLeaveRetiredDefault').remove()
            }
            else{
                $('#isYearLeaveRetiredDefault').text("Yıllık İzin Yenilenirken 20 günden yenilenecektir.")
            }
            if (data.yearLeaveDate === data.startJobDate){
                $('#yearLeaveDateIsDefault').text("Yıllık İzin Yenilenme Tarihi Varsayılan olarak İşe Başlangıç Tarihinden Alındı.") 
            }
            else{
                $('#yearLeaveDateIsDefault').text("Yıllık İzin Yenilenme Tarihi Bir Önceki Personel Kartının İşe Giriş Tarihinden Baz Alındı.") 
            }
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
        //Telefon donuşum fonksiyonu
        function formatPhoneNumber(phonenumber) {
            if (!phonenumber) return ""; // null veya undefined ise boş string döndür

            // Formatlı IBAN için desen tanımla
            let pattern = /(\d{3})(\d{3})(\d{2})(\d{2})/;

            // İstenen formata göre dönüşüm yap
            return phonenumber.replace(pattern, "$1-$2-$3-$4");
        }
        //Iban dönüşüm fonksiyonu
        function formatIBAN(iban) {
            if (!iban) return ""; // null veya undefined ise boş string döndür
            
            // Formatlı IBAN için desen tanımla
            let pattern = /(\d{2})(\d{4})(\d{4})(\d{4})(\d{4})(\d{4})(\d{2})/;

            // İstenen formata göre dönüşüm yap
            return iban.replace(pattern, "TR$1-$2-$3-$4-$5-$6-$7");
        }
        //Alacak İzin Saat bazından güne cevirme metodu
        function saatleriGunVeSaatlereCevir(saat) {
            // Toplam saatleri gün ve saatlere çevir
            let gun = Math.floor(saat / 8);
            let kalanSaat = saat % 8;

            // Sonucu döndür
            return gun + " gün " + kalanSaat.toFixed(1) + " saat";
        }
        // Şube ve pozisyon seçeneklerinin ayarlanması
        function setSelects() {
            branchSelect.empty();
            positionSelect.empty();
            departmantSelect.empty();
            $.each(data.branches, function (index, branch) {
                branchSelect.append(`<option value='${branch.id}'>${branch.name}</option>`);
            });
            $.each(data.positions, function (index, position) {
                positionSelect.append(`<option value='${position.id}'>${position.name}</option>`);
            });
            const departmantsJson = `
        [
  {
    "Name": "Yönetim",
    "Value": "Yönetim"
  },
   {
    "Name": "Satın Alma Departmanı",
    "Value": "Satın Alma Departmanı"
  },
   {
    "Name": "Muhasebe Servisi",
    "Value": "Muhasebe Servisi"
  },
   {
    "Name": "Genel Müdür",
    "Value": "Genel Müdür"
  },
   {
    "Name": "Bilgi İşlem Servisi",
    "Value": "Bilgi İşlem Servisi"
  },
   {
    "Name": "Finans Müdürü",
    "Value": "Finans Müdürü"
  },
   {
    "Name": "Santral Görevlisi",
    "Value": "Santral Görevlisi"
  },
    

  {
    "Name": "Depo",
    "Value": "Depo"
  },
  {
    "Name": "Ortaklar Servisi",
    "Value": "Ortaklar Servisi"
  },
  {
    "Name": "Otel",
    "Value": "Otel"
  },
  {
    "Name": "Vezne",
    "Value": "Vezne"
  },
  {
    "Name": "Çay Servisi",
    "Value": "Çay Servisi"
  },
  {
    "Name": "İnsan Kaynakları Departmanı",
    "Value": "İnsan Kaynakları Departmanı"
  },
  {
    "Name": "E-Ticaret",
    "Value": "E-Ticaret"
  },
  {
    "Name": "Basın ve Reklam Birimi",
    "Value": "Basın ve Reklam Birimi"
  },
  {
    "Name": "Kasap",
    "Value": "Kasap"
  },
  {
    "Name": "Şarküteri",
    "Value": "Şarküteri"
  },
  {
    "Name": "Süpermarket Sorumlusu",
    "Value": "Süpermarket Sorumlusu"
  },
  {
    "Name": "Kasiyer",
    "Value": "Kasiyer"
  },
  {
    "Name": "Ara Eleman",
    "Value": "Ara Eleman"
  },
  {
    "Name": "Kozmetik",
    "Value": "Kozmetik"
  },
  {
    "Name": "E-Ticaret Ürün Hazırlama",
    "Value": "E-Ticaret Ürün Hazırlama"
  },
  {
    "Name": "İnşaat / Teknik",
    "Value": "İnşaat / Teknik"
  },
  {
    "Name": "Kuruyemiş",
    "Value": "Kuruyemiş"
  },
  {
    "Name": "İş Takibi",
    "Value": "İş Takibi"
  },
  {
    "Name": "Araba Toplama (Sepetçi)",
    "Value": "Araba Toplama (Sepetçi)"
  },
  {
    "Name": "Personel Yemekhanesi",
    "Value": "Personel Yemekhanesi"
  },
  {
    "Name": "Kısmi Süreli",
    "Value": "Kısmi Süreli"
  },
  {
    "Name": "Avm Market Şubesi",
    "Value": "Avm Market Şubesi"
  },
  {
    "Name": "Ayazmana Şubesi",
    "Value": "Ayazmana Şubesi"
  },
  {
    "Name": "Bulvar Şubesi",
    "Value": "Bulvar Şubesi"
  },
  {
    "Name": "Burdur Şubesi",
    "Value": "Burdur Şubesi"
  },
  {
    "Name": "Çünür Şubesi",
    "Value": "Çünür Şubesi"
  },
  {
    "Name": "Dinar Şubesi",
    "Value": "Dinar Şubesi"
  },
  {
    "Name": "Gölcük Şubesi",
    "Value": "Gölcük Şubesi"
  },
  {
    "Name": "Halıkent Şubesi",
    "Value": "Halıkent Şubesi"
  },
  {
    "Name": "Iyaş Park İdari",
    "Value": "Iyaş Park İdari"
  },
  {
    "Name": "Kesikbaş Şubesi",
    "Value": "Kesikbaş Şubesi"
  },
  {
    "Name": "Kongre Merkezi",
    "Value": "Kongre Merkezi"
  },
  {
    "Name": "Nokta Şubesi",
    "Value": "Nokta Şubesi"
  },
  {
    "Name": "Akaryakıt (Shell)",
    "Value": "Akaryakıt (Shell)"
  },
  {
    "Name": "Part-Time",
    "Value": "Part-Time"
  }
]
        `
            const departmants = JSON.parse(departmantsJson);
            $.each(departmants, function (index, departmant) {
                departmantSelect.append(`<option value='${departmant.Value}'>${departmant.Value}</option>`);
            });
            branchSelect.val(data.branch_Id);
            positionSelect.val(data.position_Id);
            departmantSelect.val(data.personalDetails.departmantName)
            PersonalGroupSelect.val(data.personalDetails.personalGroup);
            EducationStatusSelect.val(data.personalDetails.educationStatus);
            MaritalStatusSelect.val(data.personalDetails.maritalStatus);
            BodySizeSelect.val(data.personalDetails.bodySize);
            BloodGroupSelect.val(data.personalDetails.bloodGroup);
        }

        

        // Emeklilik ve engellilik durumlarının işaretlenmesi
        function setCheckboxes() {
            data.personalDetails.handicapped ? $('input[name="PersonalDetails.Handicapped"]').prop('checked', true) : "";
            data.isYearLeaveRetired ? $('input[name="IsYearLeaveRetired"]').prop('checked', true) : "";
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
    function kalanIzinKumulatif(cumulativeList , isPersonalWorking) {
        let outputDiv = document.getElementById("balanceYearLeaveDetail");
        let todayDate = new Date()
        cumulativeList.forEach(i=> {
                outputDiv.innerHTML += "Yıl: " + i.year + ", Kalan: " + i.remainYearLeave  + "<br>";
        })
        if(isPersonalWorking && !cumulativeList.find(p=> p.year === todayDate.getFullYear())){
            outputDiv.innerHTML += "Yıl: " + todayDate.getFullYear() + ", Bekliyor" + "<br>";
        }
        
    }
    function fillCumulativeTable(cumulativeList,yearLeaveDate , isPersonalWorking){
        const inputSection = $('#cumulative-inputs-section');
        
        let todayDate = new Date()
        let tempDate = new Date(todayDate.getFullYear(),yearLeaveDate.getMonth(),yearLeaveDate.getDate())
        if(cumulativeList.length > 0){
            cumulativeList.forEach(p=>{
                // if (p.year === todayDate.getFullYear() && tempDate > todayDate) {
                //     let inputRow = `
                //    <tr>
                //     <input type="hidden" data-cumulativeId value="${p.id}">
                //     <td><span class="input-group-text">${p.year}</span></td>
                //     <td><input type="number" data-earnedYearLeave name="cumulativeFormulaInput" value="${p.earnedYearLeave}" class="form-control" /></td>
                //     <td><input type="number" data-remainYearLeave name="cumulativeFormulaInput" value="${p.remainYearLeave}" class="form-control" /></td>
                //     <td class="text-center"><input type="checkbox" data-isReportCompleted name="cumulativeFormulaInput" value="${p.isReportCompleted}" ${p.isReportCompleted ? "checked" : ""} class="form-check-input"/></td>
                //     <td class="text-center"><input type="checkbox" data-isNotificationExist name="cumulativeFormulaInput" value="${p.isNotificationExist}" ${p.isNotificationExist ? "checked" : ""} class="form-check-input"/></td>
                //     <td class="text-center">
                //         <div class="btn-group">
                //             <button data-cumulativeUpdateBtn type="button" class="btn btn-pill btn-sm btn-green">Güncelle</button >
                //            
                //         </div>
                //     </td>
                //    </tr>
                // `
                //     inputSection.append(inputRow)
                // }
                // else{
                //     let inputRow = `
                //     <tr>
                //         <input type="hidden" data-cumulativeId value="${p.id}">
                //         <td><span class="input-group-text">${p.year}</span></td>
                //         <td><input type="number" data-earnedYearLeave name="cumulativeFormulaInput" value="${p.earnedYearLeave}" class="form-control" /></td>
                //         <td><input type="number" data-remainYearLeave name="cumulativeFormulaInput" value="${p.remainYearLeave}" class="form-control" /></td>
                //         <td class="text-center"><input type="checkbox" data-isReportCompleted name="cumulativeFormulaInput" value="${p.isReportCompleted}" ${p.isReportCompleted ? "checked" : ""} class="form-check-input"/></td>
                //         <td class="text-center"><input type="checkbox" data-isNotificationExist name="cumulativeFormulaInput" value="${p.isNotificationExist}" ${p.isNotificationExist ? "checked" : ""} class="form-check-input"/></td>
                //         <td class="text-center"><button data-cumulativeUpdateBtn type="button" class="btn btn-pill btn-sm btn-green">Güncelle</button ></td>
                //     </tr>
                // `
                //     inputSection.append(inputRow)
                // }
                let inputRow = `
                    <tr>
                        <input type="hidden" data-cumulativeId value="${p.id}">
                        <td><span class="input-group-text">${p.year}</span></td>
                        <td><input type="number" data-earnedYearLeave name="cumulativeFormulaInput" value="${p.earnedYearLeave}" class="form-control" /></td>
                        <td><input type="number" data-remainYearLeave name="cumulativeFormulaInput" value="${p.remainYearLeave}" class="form-control" /></td>
                        <td class="text-center"><input type="checkbox" data-isReportCompleted name="cumulativeFormulaInput" value="${p.isReportCompleted}" ${p.isReportCompleted ? "checked" : ""} class="form-check-input"/></td>
                        <td class="text-center"><input type="checkbox" data-isNotificationExist name="cumulativeFormulaInput" value="${p.isNotificationExist}" ${p.isNotificationExist ? "checked" : ""} class="form-check-input"/></td>
                        <td class="text-center"><button data-cumulativeUpdateBtn type="button" class="btn btn-pill btn-sm btn-green" ${!isPersonalWorking ? "disabled" : ""}>Güncelle</button ></td>
                    </tr>
                `
                inputSection.append(inputRow)
                
                
            })
        }
        if(isPersonalWorking && !cumulativeList.find(p=> p.year === todayDate.getFullYear())){
            let inputRow = `
                   <tr>
                    <td><span class="input-group-text">${todayDate.getFullYear()}</span></td>
                    <td><span class="input-group-text">Bekliyor</span></td>
                    <td><span class="input-group-text">Bekliyor</span></td>
                    <td class="text-center"><input type="checkbox" data-isReportCompleted value="false"  class="form-check-input" disabled/></td>
                    <td class="text-center"><input type="checkbox" data-isNotificationExist value="false"  class="form-check-input" disabled/></td>
                    <td class="text-center"><button type="button" data-cumulativeShowEditBtn class="btn btn-pill btn-sm btn-yellow">Düzenlemeyi Etkinleştir</button></td>
                    </tr>
            `
            inputSection.append(inputRow)
        }
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
        $(function() { 
            // Kümülatif Güncelle butonu
            $(document).on('click', 'button[data-cumulativeUpdateBtn]', function(event) {
                let clickedButton = $(this)
                spinnerStart(clickedButton)
                let row = $(this).closest('tr'); // Tıklanan butonun bulunduğu tablo satırını bul
                let id = row.find('input[data-cumulativeId]').val();
                let year = row.find('span.input-group-text').text();
                let earnedYearLeave = row.find('input[data-earnedYearLeave]').val();
                let remainYearLeave = row.find('input[data-remainYearLeave]').val();
                let isReportCompleted = row.find('input[data-isReportCompleted]').prop('checked');
                let isNotificationExist = row.find('input[data-isNotificationExist]').prop('checked');
                let personalId = document.querySelector("input[name='ID']").value;
                let formData = {
                            ID : id,
                            Year : year,
                            EarnedYearLeave : earnedYearLeave,
                            RemainYearLeave : remainYearLeave,
                            IsReportCompleted : isReportCompleted,
                            IsNotificationExist : isNotificationExist,
                            Personal_Id : personalId
                        }
                $.ajax({
                    type: "POST",
                    url: "/personel-detaylari-kumulatif-guncelle",
                    data: formData
                }).done(function (res) {
                    spinnerEnd(clickedButton)
                    if (res.isSuccess) {
                        $('#success-modal-message').text("Kümülatif Başarılı Bir Şekilde Güncellendi.")
                        $('#success-modal').modal('show');
                        $('#success-modal').on('hidden.bs.modal', function () {
                            window.location.reload();
                        })
                    } else {
                        $('#error-modal-message').text(res.message)
                        $('#error-modal').modal('show');
                    }
                });
            });
            // Kumulatif şimdiki yıl manuel etkinleştirme aksiyonu
            $(document).on('click', 'button[data-cumulativeShowEditBtn]', function(event) {
                let clickedButton = $(this)
                let row = clickedButton.closest('tr'); // Tıklanan butonun bulunduğu tablo satırını bul
                row.find('td:nth-child(2) span').replaceWith('<input type="number" data-earnedYearLeave name="cumulativeFormulaInput" value="0" class="form-control">'); // İkinci sütundaki span'ı input ile değiştirin
                row.find('td:nth-child(3) span').replaceWith('<input type="number" data-remainYearLeave name="cumulativeFormulaInput" value="0" class="form-control">'); // Üçüncü sütundaki span'ı input ile değiştirin
                row.find('td:nth-child(4) input').prop('disabled', false); // Dördüncü sütundaki inputları etkinleştirin
                row.find('td:nth-child(5) input').prop('disabled', false); // Beşinci sütundaki inputları etkinleştirin
                clickedButton.replaceWith('<button type="button" data-cumulativeAddBtn class="btn btn-pill btn-sm btn-blue">Manuel Yıllık İzin Ekle</button>')
                $(document).on('click', 'button[data-cumulativeAddBtn]', function(event) {
                    let clickedButton = $(this)
                    let row = clickedButton.closest('tr'); // Tıklanan butonun bulunduğu tablo satırını bul
                    $('#cumulative-add-modal').modal('show')
                    $('#cumulative-modal-button-add').click(function () {
                        let year = row.find('span.input-group-text').text();
                        let earnedYearLeave = row.find('input[data-earnedYearLeave]').val();
                        let remainYearLeave = row.find('input[data-remainYearLeave]').val();
                        let isReportCompleted = row.find('input[data-isReportCompleted]').prop('checked');
                        let isNotificationExist = row.find('input[data-isNotificationExist]').prop('checked');
                        let personalId = document.querySelector("input[name='ID']").value;
                        let formData = {
                            Year : year,
                            EarnedYearLeave : earnedYearLeave,
                            RemainYearLeave : remainYearLeave,
                            IsReportCompleted : isReportCompleted,
                            IsNotificationExist : isNotificationExist,
                            Personal_Id : personalId
                        }
                        let modalClickButton = $(this)
                        spinnerStart(modalClickButton)
                        $.ajax({
                            type: "POST",
                            url: "/personel-detaylari-kumulatif-guncelle",
                            data: formData
                        }).done(function (res) {
                            spinnerEnd(modalClickButton)
                            if (res.isSuccess) {
                                $('#success-modal-message').text("Kümülatif Başarılı Bir Şekilde Eklendi.")
                                $('#success-modal').modal('show');
                                $('#success-modal').on('hidden.bs.modal', function () {
                                    window.location.reload();
                                })
                            } else {
                                $('#error-modal-message').text(res.message)
                                $('#error-modal').modal('show');
                            }
                        });
                    });
                        
                    
                });
                
            });
        });
        //Düzenlemeyi Etkinleştir Butonu Tıklandığında
        enableEditButton.on('click', function () {
            let inputs = $('#updatePersonalForm input');
            inputs.prop('disabled', false);
            $('[data-hourInput]').prop('disabled', false);
            $('[data-addHour]').prop('disabled', false);
            $('[data-removeHour]').prop('disabled', false);

            $('#updatePersonalForm textarea').prop('disabled', false);
            $('#updatePersonalForm select').prop('disabled', false);
            new TomSelect(positionSelect); // Seçilen select'i alın
            new TomSelect(branchSelect); // Seçilen select'i alın
            new TomSelect(departmantSelect); // Seçilen select'i alın
            new TomSelect(PersonalGroupSelect);
            new TomSelect(EducationStatusSelect); // Seçilen select'i alın
            new TomSelect(MaritalStatusSelect); // Seçilen select'i alın
            new TomSelect(BodySizeSelect); // Seçilen select'i alın
            new TomSelect(BloodGroupSelect); // Seçilen select'i alın
            enableEditButton.prop('disabled', true);
            updateButton.prop('disabled', false);
        });
        //Alacak İzin Saat Ekle tıklandığında çalışan metod
        $('[data-addHour]').on('click', function () {
            if ($('[data-hourInput]').val() !== '') {
                $('[data-takenLeave]').val(function (index, currentValue) {
                    return (parseFloat(currentValue) + parseFloat($('[data-hourInput]').val())).toFixed(1);
                });
            }
            $('[data-hourInput]').val("");
        });
        //Alacak İzin Saat Çıkart tıklandığında çalışan metod
        $('[data-removeHour]').on('click', function () {
            if ($('[data-hourInput]').val() !== '') {
                $('[data-takenLeave]').val(function (index, currentValue) {
                    return (parseFloat(currentValue) - parseFloat($('[data-hourInput]').val())).toFixed(1);
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
        updateButton.on('click', function () {
            spinnerStart(updateButton)
            let formData = $("#updatePersonalForm").serializeArray();
            let foodAidValue = formData.find(item => item.name === "FoodAid").value
            if (!checkRequiredFields()) {
                spinnerEnd(updateButton)
                $('#error-modal-message').text("Lütfen Zorunlu Alanları Girdiğinizden Emin Olunuz.")
                $('#error-modal').modal('show');
                return; // Fonksiyondan çık
            }
            if (foodAidValue < 0) {
                spinnerEnd(updateButton)
                $('#error-modal-message').text("Gıda Yardımı 0 dan küçük olamaz.")
                $('#error-modal').modal('show');
                return true;
            }
            if(formData.some(p=> p.name === "IsYearLeaveRetired" && p.value === "on") && !formData.some(p=> p.name === "RetiredOrOld" && p.value === "on")){
                spinnerEnd(updateButton)
                $('#error-modal-message').text("Yıllık izin yenilenirken emeklilik tarihi baz alınsın işaretlenir ise emeklilik durumu açık ve tarih girilmesi zorunludur.")
                $('#error-modal').modal('show');
                return true;
            }
            
            formData.forEach(function (f) {
                if (f.value === "on") {
                    f.value = true;
                }
                if (f.name === "Phonenumber"){
                    if (f.value === "___-___-__-__"){
                        f.value = null
                    }
                    else{
                        f.value = f.value.replace(/-/g, "");
                    }
                }
                else if (f.name === "PersonalDetails.IBAN"){
                    if (f.value === "TR__-____-____-____-____-____-__"){
                        f.value = null
                    }
                    else{
                        f.value = f.value.replace(/^TR|-/g, "");
                    }
                }
            });
            //Formun id'sini kullanarak formu gönder
            $.ajax({
                type: "POST",
                url: "/personel-detaylari",
                data: formData // Form verilerini al
            }).done(function (res) {
                spinnerEnd(updateButton)
                if (res.isSuccess) {
                    $('#success-modal-message').text("Personel Başarılı Bir Şekilde Güncellendi.")
                    $('#success-modal').modal('show');
                    $('#success-modal').on('hidden.bs.modal', function () {
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
            spinnerStart($("#deleteSubmitBtn"))
            $.ajax({
                type: "POST",
                url: `/personel-sil?id=${$('input[name="ID"]').val()}`
            }).done(function (res) {
                spinnerEnd($("#deleteSubmitBtn"))
                if (res.isSuccess) {
                    $('#success-modal-message').text("Personel Başarılı Bir Şekilde Silindi")
                    $('#success-modal').modal('show')
                    $('#success-modal').on('hidden.bs.modal', function () {
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
            spinnerStart($('#istenCikarSubmitButton'))
            let formData = $("#istenCikarForm").serializeArray();
            let hasEmptyField = false;
            $("#istenCikarForm input[name='ID'], #istenCikarForm input[name='EndJobDate']").each(function () {
                if ($(this).val().trim() === "") {
                    hasEmptyField = true;
                    return false; // Döngüyü sonlandır
                }
            });
            if (hasEmptyField) {
                spinnerEnd($('#istenCikarSubmitButton'))
                $('#error-modal-message').text("Lütfen Çıkış Tarihini Seçtiğinizden Emin Olunuz!")
                $('#error-modal').modal('show');
            } else {
                $.ajax({
                    type: "POST",
                    url: `/personel-durumu`,
                    data: formData
                }).done(function (res) {
                    spinnerEnd($('#istenCikarSubmitButton'))
                    if (res.isSuccess) {
                        $('#success-modal-message').text("Personel Başarılı Bir Şekilde İşten Çıkarıldı.")
                        $('#success-modal').modal('show')
                        $('#success-modal').on('hidden.bs.modal', function () {
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
            spinnerStart($('#iseGeriAlSubmitButton'))
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
                spinnerEnd($('#iseGeriAlSubmitButton'))
                $('#error-modal-message').text("Lütfen İşe Giriş Tarihini Seçtiğinizden Emin Olunuz!")
                $('#error-modal').modal('show');
            } else {
                $.ajax({
                    type: "POST",
                    url: `/personel-durumu`,
                    data: formData
                }).done(function (res) {
                    spinnerEnd($('#iseGeriAlSubmitButton'))
                    if (res.isSuccess) {
                        $('#success-modal-message').text("Personel Başarılı Bir Şekilde Geri İşe Alındı.")
                        $('#success-modal').modal('show')
                        $('#success-modal').on('hidden.bs.modal', function () {
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

