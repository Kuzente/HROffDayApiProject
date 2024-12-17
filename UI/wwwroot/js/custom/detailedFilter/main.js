document.addEventListener('DOMContentLoaded',function () {
    //<--!Static Data Section -->
    let branchStatusEnum = [
        { "Name": "Aktif", "Value": "Online" },
        { "Name": "Pasif", "Value": "Offline" },
        { "Name": "Silinen", "Value": "Archive" },
    ];
    let positionStatusEnum = [
        { "Name": "Aktif", "Value": "Online" },
        { "Name": "Pasif", "Value": "Offline" },
        { "Name": "Silinen", "Value": "Archive" },
    ];
    let personalStatusEnum = [
        { "Name": "Aktif Çalışan", "Value": "Online" },
        { "Name": "İşten Çıkarılan", "Value": "Offline" },
        { "Name": "Silinen", "Value": "Deleted" },
    ];
    let bloodGroupList;
    let bodySize;
    let educationStatus;
    let maritalStatus;
    let personalGroup;
    let departments;
    let datatableLanguage;
    let allBranchList;
    let allPositionList;
    moment.locale('tr');
    
   
    $.ajax({
        type: "GET",
        url: "json/datatable-tr.json",
        dataType: 'json',
        success: function (data) {
            datatableLanguage = data
        },
        error: function () {
            $('#error-modal-message').text("Tabloya ait türkçe veri seti getirilirken bir hata oluştur!!")
            $('#error-modal').modal('show')
        }
    });
    $.ajax({
        type: "GET",
        url: "/query/tum-subeler?$select=Name,ID,Status&$orderby=Name asc",
        dataType: 'json',
        success: function (data) {
            allBranchList = data;
        },
        error: function () {
            $('#error-modal-message').text("Şube verileri getirilirken bir hata oluştu!!")
            $('#error-modal').modal('show')
        }
    });
    $.ajax({
        type: "GET",
        url: "/query/tum-unvanlar?$select=Name,ID,Status&$orderby=Name asc",
        dataType: 'json',
        success: function (data) {
            allPositionList = data;
        },
        error: function () {
            $('#error-modal-message').text("Ünvan verileri getirilirken bir hata oluştu!!")
            $('#error-modal').modal('show')
        }
    });
   $.ajax({
       type: "GET",
       url: "json/personal-selects-data.json",
       dataType: 'json',
       success: function (data) {
           bloodGroupList = data.bloodGroup;
           bodySize = data.bodySize;
           educationStatus = data.educationStatus;
           maritalStatus = data.maritalStatus
           personalGroup = data.personalGroup
           departments = data.departments
       },
       error: function () {
           $('#error-modal-message').text("Personel Detaylarına ait kan grupları, personel grupları, eğitim durumları vs. getirilirken bir hata oluştu!")
           $('#error-modal').modal('show')  
       }
   });
    // <--Static Data Section -->
    //Tablonun Değişmesi Durumunda Çalışan Fonksiyon
    $(document).on('change', '#entitySelect', function() {
        let propertyViewSelectElement;
        let propertyViewSelectDiv = $('#propertiesViewSelectDiv')
        let selectedEntityName = $(this).find('option:selected').text()
        let selectedEntityValue = $(this).find('option:selected').val()
        const addFilterBtn = $('#addFilterBtn')
        const sendFilterBtn = $('#sendFiltersBtn')
        let filtersRowSection = $('#filtersRowSection')
        filtersRowSection.empty()
        let alertSection = $('#alert_section')
        alertSection.empty()
        propertyViewSelectDiv.empty()
        addFilterBtn.addClass('d-none')
        sendFilterBtn.addClass('d-none')
        if (selectedEntityValue){
            addFilterBtn.removeClass('d-none')
            sendFilterBtn.removeClass('d-none')
            if (selectedEntityValue === "Personal") {
                alertSection.append(`
                        <div class="alert alert-warning " role="alert">
                                <h4 class="alert-title">Bilgilendirme!</h4>
                                <div class="text-secondary">
                                    Personeller Tablosunda yaptığınız filtreleme işlemlerinde varsayılan olarak:
                                    <ul>
                                        <li><b>Aktif</b> Çalışan Personeller</li>
                                        <li><b>Aktif</b> Şubeler</li>
                                        <li><b>Aktif</b> Ünvanlar</li>
                                    </ul>
                                     üzerinde arama yapılmaktadır. Bunlar dışında bir arama yapmak isterseniz, lütfen filtre ekle bölümünden ilgili alanları(<b>Personel Durumu veya Şube Durumu veya Ünvan Durumu</b>) seçiniz!
                                </div>
                            </div>
                `)
                propertyViewSelectElement = `
                <label for="propertiesViewSelect">Getirilecek Özellikler:</label>
                                    <select id="propertiesViewSelect" class="" name="propertiesView" multiple placeholder="Getirmek istediğiniz kolonları seçiniz!">
                                        <optgroup label="Personel Bilgi Kolonları">
                                            <option value="NameSurname">Adı-Soyadı</option>
                                            <option class="personalDetailProp" value="PersonalDetails/BirthPlace">Doğum Yeri</option>
                                            <option value="IdentificationNumber">Tc Kimlik Numarası</option>
                                            <option class="personalDetailProp" value="PersonalDetails/SskNumber">SSK Numarası</option>
                                            <option class="personalDetailProp" value="PersonalDetails/SgkCode">SGK Kodu</option>
                                            <option value="Phonenumber">Telefon</option>
                                            <option class="personalDetailProp" value="PersonalDetails/FatherName">Baba Adı</option>
                                            <option class="personalDetailProp" value="PersonalDetails/MotherName">Anne Adı</option>
                                            <option class="personalDetailProp" value="PersonalDetails/IBAN">IBAN</option>
                                            <option class="personalDetailProp" value="PersonalDetails/BankAccount">Banka Hesap No</option>
                                            <option class="personalDetailProp" value="PersonalDetails/Address">Adres</option>
                                            <option value="Gender">Cinsiyet</option>
                                            <option class="personalDetailProp" value="PersonalDetails/DepartmantName">Departman</option>
                                            <option class="personalDetailProp" value="PersonalDetails/BloodGroup">Kan Grubu</option>
                                            <option class="personalDetailProp" value="PersonalDetails/MaritalStatus">Medeni Durumu</option>
                                            <option class="personalDetailProp" value="PersonalDetails/EducationStatus">Eğitim Durumu</option>
                                            <option class="personalDetailProp" value="PersonalDetails/PersonalGroup">Personelin Bulunduğu Grup</option>
                                            <option class="personalDetailProp" value="PersonalDetails/BodySize">Beden Ölçüsü</option>
                                            <option value="RegistirationNumber">Sicil Numarası</option>
                                            <option value="FoodAid">Gıda Yardımı Miktarı</option>
                                            <option class="personalDetailProp" value="PersonalDetails/Salary">Maaş</option>
                                            <option value="BirthDate">Doğum Tarihi</option>
                                            <option value="StartJobDate">İşe Başlama Tarihi</option>
                                            <option value="EndJobDate">İşten Çıkış Tarihi</option>
                                            <option value="RetiredDate">Emeklilik Tarihi</option>
                                            <option value="FoodAidDate">Gıda Yardımı Yenilenme Tarihi</option>
                                            <option value="CreatedAt">Oluşturulma Tarihi</option>
                                            <option value="RetiredOrOld">Emeklilik Durumu</option>
                                            <option class="personalDetailProp" value="PersonalDetails/Handicapped">Engellilik Durumu</option>
                                            <option value="IsBackToWork">İşe Geri Alınma Durumu</option>
                                        </optgroup>
                                        <optgroup label="Personel İzin Kolonları">
                                            <option value="TotalYearLeave">Toplam Hak Edilen Yıllık İzin Miktarı</option>
                                            <option value="UsedYearLeave">Toplam Kullanılan Yıllık İzin Miktarı</option>
                                            <option value="TotalTakenLeave">Toplam Alacak İzin Miktarı (Saat)</option>
                                            <option value="YearLeaveDate">Yıllık İzin Yenilenme Tarihi</option>
                                            <option value="IsYearLeaveRetired">Yıllık İzini Emeklilik Durumu İle Yenilenenler</option>
                                            <option class="personalCumulativeProp" value="PersonalCumulatives/EarnedYearLeave">Yıllara Göre Kümülatif Hak Edilen İzin Sayısı</option>
                                            <option class="personalCumulativeProp" value="PersonalCumulatives/RemainYearLeave">Yıllara Göre Kümülatif Kalan İzin Sayısı</option>
                                        </optgroup>
                                        <optgroup label="Şube Kolonları">
                                            <option class="branchProp" value="Branch/Name">Şube Adı</option>
                                        </optgroup>
                                        <optgroup label="Ünvan Kolonları">
                                            <option class="positionProp" value="Position/Name">Ünvan / Görev Adı</option>
                                        </optgroup>
                                    </select>
            `
                propertyViewSelectDiv.append(propertyViewSelectElement);
                let propertiesViewSelect = new TomSelect($('#propertiesViewSelect'),{
                    plugins: ['drag_drop'],
                    maxOptions : null,
                });
                propertiesViewSelect.clear()
            }
        }
        
        //Seçili getirilecek kolon yerini sıfırlar
        
        
        
        //Datatable alanınını sıfırlar
        $('#results').empty().removeClass()
    });
    
    
    //Filtre Ekle butonuna ait function
    $('#addFilterBtn').click(function() {
        let selectedEntityVal = $('#entitySelect').find('option:selected').val()
        let filterSelectElement;
        let selectId = "select_" + generateGUID()
        if (selectedEntityVal === 'Personal'){
            filterSelectElement = `
                    <div class="row mt-3 filter-row">
                        <div class="col d-flex align-items-end">
                            <button class="btn btn-danger btn-icon remove-filter">x</button>
                            <div class="ms-2 d-inline-block">
                                <label class="form-label">Filtre Uygulamak İstediğiniz Kolonu Seçiniz</label>
                                <select class="form-select propertySelect" placeholder="Kolon Seçiniz!!!" name="propertyName" id="${selectId}">
                                    <option value=""></option>
                                    <optgroup label="Personel Metin Kolonları">
                                        <option data-type="Text" value="NameSurname">Adı-Soyadı</option>
                                        <option data-type="Text" value="PersonalDetails/BirthPlace">Doğum Yeri</option>
                                        <option data-type="Text" value="IdentificationNumber">Tc Kimlik Numarası</option>
                                        <option data-type="Text" value="PersonalDetails/SskNumber">SSK Numarası</option>
                                        <option data-type="Text" value="PersonalDetails/SgkCode">SGK Kodu</option>
                                        <option data-type="Text" value="Phonenumber">Telefon</option>
                                        <option data-type="Text" value="PersonalDetails/FatherName">Baba Adı</option>
                                        <option data-type="Text" value="PersonalDetails/MotherName">Anne Adı</option>
                                        <option data-type="Text" value="PersonalDetails/IBAN">IBAN</option>
                                        <option data-type="Text" value="PersonalDetails/BankAccount">Banka Hesap No</option>
                                        <option data-type="Text" value="PersonalDetails/Address">Adres</option>
                                    </optgroup>
                                    <optgroup label="Personel Liste Kolonları">
                                        <option data-type="List" value="Status">Personel Durumu</option>
                                        <option data-type="List" value="Gender">Cinsiyet</option>
                                        <option data-type="List" value="PersonalDetails/DepartmantName">Departman</option>
                                        <option data-type="List" value="PersonalDetails/BloodGroup">Kan Grubu</option>
                                        <option data-type="List" value="PersonalDetails/MaritalStatus">Medeni Durumu</option>
                                        <option data-type="List" value="PersonalDetails/EducationStatus">Eğitim Durumu</option>
                                        <option data-type="List" value="PersonalDetails/PersonalGroup">Personelin Bulunduğu Grup</option>
                                        <option data-type="List" value="PersonalDetails/BodySize">Beden Ölçüsü</option>
                                    </optgroup>
                                    <optgroup label="Personel Sayı Kolonları">
                                        <option data-type="Number" value="TotalYearLeave">Hak Edilen Yıllık İzin Miktarı</option>
                                        <option data-type="Number" value="UsedYearLeave">Kullanılan Yıllık İzin Miktarı</option>
                                        <option data-type="Number" value="RegistirationNumber">Sicil Numarası</option>
                                        <option data-type="Number" value="FoodAid">Gıda Yardımı Miktarı</option>
                                    </optgroup>
                                    <optgroup label="Personel Ondalıklı Sayı Kolonları">
                                        <option data-type="Double" value="TotalTakenLeave">Mevcut Alacak İzin Miktarı</option>
                                        <option data-type="Double" value="PersonalDetails/Salary">Maaş</option>
                                    </optgroup>
                                    <optgroup label="Personel Tarih Kolonları">
                                        <option data-type="Date" value="BirthDate">Doğum Tarihi</option>
                                        <option data-type="Date" value="StartJobDate">İşe Başlama Tarihi</option>
                                        <option data-type="Date" value="EndJobDate">İşten Çıkış Tarihi</option>                                  
                                        <option data-type="Date" value="YearLeaveDate">Yıllık İzin Yenilenme Tarihi</option>
                                        <option data-type="Date" value="RetiredDate">Emeklilik Tarihi</option>
                                        <option data-type="Date" value="FoodAidDate">Gıda Yardımı Yenilenme Tarihi</option>
                                        <option data-type="Date" value="CreatedAt">Oluşturulma Tarihi</option>
                                    </optgroup>
                                    <optgroup label="Personel Seçimli Kolonları">
                                        <option data-type="Radio" value="RetiredOrOld">Emeklilik Durumu</option>
                                        <option data-type="Radio" value="IsYearLeaveRetired">Yıllık İzini Emeklilik Durumu İle Yenilenenler</option>
                                        <option data-type="Radio" value="PersonalDetails/Handicapped">Engellilik Durumu</option>
                                        <option data-type="Radio" value="IsBackToWork">İşe Geri Alınma Durumu</option>
                                    </optgroup>  
                                    <optgroup label="Şube Kolonları">
                                        <option data-type="List" value="Branch/ID">Şube Adı</option>
                                        <option data-type="List" value="Branch/Status">Şube Durumu</option>
                                    </optgroup>
                                    <optgroup label="Ünvan Kolonları">
                                        <option data-type="List" value="Position/ID">Ünvan / Görev Adı</option>
                                        <option data-type="List" value="Position/Status">Ünvan / Görev Durumu</option>
                                    </optgroup>
                                   </select>
                            </div>
                            <div class="ms-5 d-inline-block border-start border-3 border-dark ps-4 filtersDetailsSection">
                            
                            </div>
                        </div>
                    </div>`; 
                   }
        $('#filtersRowSection').append(filterSelectElement);
        let filterSelect = new TomSelect($(`#${selectId}`),{
            maxOptions : null,
        });
        filterSelect.clear();
        
    });
    //Row silme işlemine ait function
    $(document).on('click', '.remove-filter', function() {
        $(this).closest('.row').remove();
    });
    //Entitylere ait propertyler arasında değişiklik yapıldığında çalışan function
    $(document).on('change', '.propertySelect', function() {
        let selectGuid = $(this).attr('id')
        let selectedType = $(this).find('option:selected').data('type');
        let selectedPropName = $(this).find('option:selected').text()
        let selectedPropValue = $(this).find('option:selected').val()
        let filtersDetailsSection = $(this).closest(".row").find('.filtersDetailsSection')
        filtersDetailsSection.empty()
        if (selectedType === 'Text') {
            let input = `
                <label class="form-label">${selectedPropName} içerisinde arama yapın.</label>
                <select multiple class="form-select TextSelect" placeholder="${selectedPropName} Giriniz" name="TextSelect">
                </select>
            `;
            filtersDetailsSection.append(input)
            new TomSelect(filtersDetailsSection.find('select'),{
                create: true,
                maxOptions: null,
                onItemRemove: function(value) {
                    this.removeOption(value); // Seçenekten kaldır
                }
            })
        }
        else if (selectedType === 'List') {
            let input = `
                <label class="form-label">${selectedPropName} göre seçim yapınız.</label>
                <select multiple class="form-select ListSelect" placeholder="${selectedPropName} Seçin" name="ListSelect">
                           <option value=""></option>
                </select>
            `;
            filtersDetailsSection.append(input)
            let selectTom = new TomSelect(filtersDetailsSection.find('select'),{
                maxOptions : null
            });
            let printArray;
            let printBranchOrPositionArray;
            switch (selectedPropValue) {
                case "Gender":
                    printArray = [{Name: "Erkek", Value: "Erkek"}, {Name: "Kadın", Value: "Kadın"}]
                    break;
                case "PersonalDetails/BloodGroup":
                    printArray = bloodGroupList
                    break;
                case "PersonalDetails/MaritalStatus" :
                    printArray = maritalStatus
                    break;
                case "PersonalDetails/PersonalGroup" :
                    printArray = personalGroup
                    break;
                case "PersonalDetails/EducationStatus" :
                    printArray = educationStatus
                    break;
                case "PersonalDetails/BodySize" :
                    printArray = bodySize
                    break;
                case "PersonalDetails/DepartmantName" :
                    printArray = departments
                    break;
                case "Status" :
                    printArray = personalStatusEnum
                    break;
                case "Branch/ID" :
                    printBranchOrPositionArray = allBranchList
                    break;
                case "Branch/Status" :
                    printArray = branchStatusEnum
                    break;
                case "Position/ID" :
                    printBranchOrPositionArray = allPositionList
                    break;
                case "Position/Status" :
                    printArray = positionStatusEnum
                    break;
            }
            $.each(printArray, function (index, item) {
                selectTom.addOption({
                    value: item.Value,
                    text: item.Name
                });
            });
            $.each(printBranchOrPositionArray, function (index, item) {
                selectTom.addOption({
                    value: item.ID,
                    text: item.Name + (item.Status === 0 ? "" : (item.Status === 1 ? " (Pasif)" : (item.Status === 2 ? " (Silinmiş)" : "")))
                });
            });
            
        }
        else if (['Number', 'Double', 'Date'].includes(selectedType)){
            let comparisonSelect = `
                <div class="d-inline-block">
                    <label class="form-label">${selectedPropName} göre filtre uygulayınız.</label>
                    <select class="form-select comparisonSelect" data-property-type="${selectedType}" placeholder="Karşılaştırma Seçin" name="comparisonType">
                             <option data-propname="${selectedPropValue}" value=""></option>
                             <option data-propname="${selectedPropValue}" value="eq">Eşit</option>
                             <option data-propname="${selectedPropValue}" value="ne">Eşit Değil</option>
                             <option data-propname="${selectedPropValue}" value="gt">Büyük</option>
                             <option data-propname="${selectedPropValue}" value="ge">Büyük Eşit</option>
                             <option data-propname="${selectedPropValue}" value="lt">Küçük</option>
                             <option data-propname="${selectedPropValue}" value="le">Küçük Eşit</option>
                             <option data-propname="${selectedPropValue}" value="between">Arasında</option>
                    </select> 
                </div>
            `;
            filtersDetailsSection.append(comparisonSelect)
            new TomSelect(filtersDetailsSection.find('select'))
        }
        else if (selectedType === 'Radio'){
            if (selectedPropValue === 'RetiredOrOld'){
                let radios = `
                <label class="form-label">${selectedPropName} seçimi yapınız.</label>
                <div class="row">
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="true" class="form-check-input" id="Emekli${selectGuid}">
                    <label class="form-check-label" for="Emekli${selectGuid}">Emekli</label>
                </div>
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="false" class="form-check-input" id="Normal${selectGuid}">
                    <label class="form-check-label" for="Normal${selectGuid}">Normal</label>
                </div>
                </div>
            `;
                filtersDetailsSection.append(radios) 
            }
            else if (selectedPropValue === 'IsYearLeaveRetired'){
                let radios = `
                <label class="form-label">${selectedPropName} seçimi yapınız.</label>
                <div class="row">
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="true" class="form-check-input" id="True${selectGuid}">
                    <label class="form-check-label" for="True${selectGuid}">Emeklilik ile yenilenen</label>
                </div>
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="false" class="form-check-input" id="False${selectGuid}">
                    <label class="form-check-label" for="False${selectGuid}">Normal yenilenen</label>
                </div>
                </div>
            `;
                filtersDetailsSection.append(radios)
            }
            else if (selectedPropValue === 'PersonalDetails/Handicapped'){
                let radios = `
                <label class="form-label">${selectedPropName} seçimi yapınız.</label>
                <div class="row">
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="true" class="form-check-input" id="True${selectGuid}">
                    <label class="form-check-label" for="True${selectGuid}">Engelli</label>
                </div>
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="false" class="form-check-input" id="False${selectGuid}">
                    <label class="form-check-label" for="False${selectGuid}">Engelsiz</label>
                </div>
                </div>
            `;
                filtersDetailsSection.append(radios)
            }
            else if (selectedPropValue === 'IsBackToWork'){
                let radios = `
                <label class="form-label">${selectedPropName} seçimi yapınız.</label>
                <div class="row">
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="true" class="form-check-input" id="True${selectGuid}">
                    <label class="form-check-label" for="True${selectGuid}">İşe Geri Alınan</label>
                </div>
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="false" class="form-check-input" id="False${selectGuid}">
                    <label class="form-check-label" for="False${selectGuid}">İşe Geri Alınmamış</label>
                </div>
                </div>
            `;
                filtersDetailsSection.append(radios)
            }
            
        }
        else {
        }
    });
    //Numara seçimlerinde karşılaştırma selecti değişme functionu
    $(document).on('change', '.comparisonSelect', function() {
        let propertyType = $(this).data('property-type')
        let selectedValue = $(this).find('option:selected').val();
        let filtersDetailsSection = $(this).closest(".row").find('.filtersDetailsSection')
        let comparisonSection = $(this).closest(".row").find('.comparisonSection')
        comparisonSection.remove()
        if (!selectedValue){
            
        }
        else if(selectedValue === 'between'){
            
            let input1 = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input 
                type="${propertyType === 'Date' ? "date" : "number"}" 
                name="${propertyType === 'Date' ? "Date" : "Amount"}" 
                class="form-control ${propertyType === 'Double' ? 'input-double' : ''}" 
                placeholder="${propertyType === 'Date' ? "Tarih 1" : "Miktar 1"}"
                >
            </div>
            `;
            let input2 = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input 
                type="${propertyType === 'Date' ? "date" : "number"}" 
                name="${propertyType === 'Date' ? "Date" : "Amount"}" 
                class="form-control ${propertyType === 'Double' ? 'input-double' : ''}" 
                placeholder="${propertyType === 'Date' ? "Tarih 2" : "Miktar 2"}"
                >
            </div>
            `;
            filtersDetailsSection.append(input1)
            filtersDetailsSection.append(input2)
            if (propertyType === 'Date'){
                flatpickr(filtersDetailsSection.find('input[type="date"]'), {
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
            
        }
        else{
            let input = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input 
                type="${propertyType === 'Date' ? "date" : "number"}" 
                name="${propertyType === 'Date' ? "Date" : "Amount"}" 
                class="form-control ${propertyType === 'Double' ? 'input-double' : ''}" 
                placeholder="${propertyType === 'Date' ? "Tarih" : "Miktar"}"
                >
            </div>
            `;
            filtersDetailsSection.append(input)
            if (propertyType === 'Date'){
                flatpickr(filtersDetailsSection.find('input[type="date"]'), {
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
        }
    });
    //Double inputları regex function
    $(document).on('keypress', 'input.input-double', function(event) {
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
    //Formu gönderme işlemlerinin yürütüldüğü kısım
    $('#sendFiltersBtn').click(function () {
        let isFormValid = true;
        let tableName = $('select[name="entityName"]').find('option:selected').val();
        if (tableName === "") { //Entity seçilen select boş ise hata dön
            $('#error-modal-message').text("Lütfen tablo seçtiğinizden emin olunuz!")
            $('#error-modal').modal('show')
            isFormValid = false
            return false;
        }

        if ($('#propertiesViewSelect option:selected').length === 0) { //Kolon seçilen select boş ise hata dön
            $('#error-modal-message').text("Lütfen getirilecek kolon seçtiğinizden emin olunuz!")
            $('#error-modal').modal('show')
            isFormValid = false
            return false;
        } else {
            // Tüm seçili değerleri kontrol et
            let allPersonalCumulatives = true;
            $('#propertiesViewSelect option:selected').each(function () {
                if (!$(this).val().startsWith("PersonalCumulatives")) {
                    allPersonalCumulatives = false;
                }
            });

            // Eğer sadece PersonalCumulatives ile başlayanlar varsa hata dön
            if (allPersonalCumulatives) {
                $('#error-modal-message').text("Lütfen Kümülatif Sütünları dışında en az bir kolon seçtiğinizden emin olunuz!");
                $('#error-modal').modal('show');
                isFormValid = false;
                return false;
            }
        }

        let odataFilters = [];
        //Filtreleme bölümü validasyon ve filter query ayarlama kısımları
        $('#filtersRowSection .filter-row').each(function () {
            let propertySelect = $(this).find('.propertySelect');
            let selectedType = propertySelect.find('option:selected').data('type');
            let selectedPropValue = propertySelect.val();
            let selectedPropName = propertySelect.find('option:selected').text();
            if (!selectedType) {
                $('#error-modal-message').text(`Lütfen filtrelenecek kolon seçtiğinizden emin olunuz!`)
                $('#error-modal').modal('show')
                isFormValid = false;
                return false;
            }
            else if (selectedType === 'Text') {
                let searchValues = []
                $(this).find('select[name="TextSelect"] option:selected').each(function () {
                    searchValues.push($(this).val());
                });
                if (searchValues.length === 0) {
                    $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                    $('#error-modal').modal('show')
                    isFormValid = false;
                    return false;
                }
                else {
                    let filterConditions = searchValues.map(value => `contains(${selectedPropValue}, '${value}')`);
                    let combinedFilter = filterConditions.join(' or ');
                    odataFilters.push(`(${combinedFilter})`);
                }

            }
            else if (selectedType === "List") {
                let selectedValues = [];
                $(this).find('select[name="ListSelect"] option:selected').each(function () {
                    selectedValues.push($(this).val());
                });
                if (selectedValues.length === 0) {
                    $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                    $('#error-modal').modal('show')
                    isFormValid = false;
                    return false;
                }
                else {
                    let filterCondition = selectedValues.map(value => {
                        let isGuid = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(value);
                        if (isGuid) {
                            return `${selectedPropValue} eq ${value}`;
                        } else {
                            return `${selectedPropValue} eq '${value}'`;
                        }
                    }).join(' or ');
                    odataFilters.push(`(${filterCondition})`);
                }

            }
            else if (['Number', 'Double', 'Date'].includes(selectedType)) {
                let comparisonType = $(this).find('.comparisonSelect').val();
                if (!comparisonType) {
                    $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                    $('#error-modal').modal('show')
                    isFormValid = false;
                    return false;
                }
                if (comparisonType === 'between') {
                    let amounts = $(this).find(selectedType === 'Date' ? 'input[name="Date"]' : 'input[name="Amount"]');
                    let amount1 = $(amounts[0]).val();
                    let amount2 = $(amounts[1]).val();
                    if (!amount1 || !amount2) {
                        $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                        $('#error-modal').modal('show')
                        isFormValid = false;
                        return false;
                    }
                    if (selectedType === 'Date') {
                        let date1 = new Date(amount1);
                        let date2 = new Date(amount2);
                        if (isNaN(date1) || isNaN(date2)) {
                            $('#error-modal-message').text(`Lütfen geçerli bir tarih giriniz!`);
                            $('#error-modal').modal('show');
                            isFormValid = false;
                            return false;
                        }
                        //minAmount = date1 < date2 ? date1.toISOString() : date2.toISOString();
                        //maxAmount = date1 > date2 ? date1.toISOString() : date2.toISOString();
                        // Querye yazılırken herhangi bir iso dönüştürme işlemi yapmıyoruz direk yazıyoruz örneğin 2024-04-01
                        minAmount = amount1 < amount2 ? amount1 : amount2;
                        maxAmount = amount1 > amount2 ? amount1 : amount2;
                    } else {
                        minAmount = Math.min(amount1, amount2);
                        maxAmount = Math.max(amount1, amount2);
                    }
                    odataFilters.push(`(${selectedPropValue} ge ${minAmount} and ${selectedPropValue} le ${maxAmount})`);
                } else {
                    let amount = $(this).find(selectedType === 'Date' ? 'input[name="Date"]' : 'input[name="Amount"]').val();
                    if (!amount) {
                        $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                        $('#error-modal').modal('show')
                        isFormValid = false;
                        return false;
                    }
                    odataFilters.push(`(${selectedPropValue} ${comparisonType} ${amount})`);
                }
            }
            else if (selectedType === 'Radio') {
                let checkedValue = $(this).find('input[type="radio"]:checked').val()
                if (!checkedValue) {
                    $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin seçeneklerinin seçili olduğundan emin olunuz!`)
                    $('#error-modal').modal('show')
                    isFormValid = false;
                    return false;
                } else {
                    odataFilters.push(`${selectedPropValue} eq ${checkedValue}`);
                }
            }

        });

        if (isFormValid) { //validasyonlar ok mi kontrolu
            $('#mainDiv').addClass('d-none');
            $('#page-loader').removeClass('d-none')
            let personalProperties = [];
            let personalDetailsProperties = [];
            let branchProperties = [];
            let positionProperties = [];
            let personalCumulativeProperties = [];
            let tableDic = {}
            $('#propertiesViewSelect option:selected').each(function () {
                if ($(this).hasClass('personalDetailProp')) {
                    personalDetailsProperties.push($(this).val().split('/')[1]);
                    tableDic[$(this).val()] = {
                        Text: $(this).text(),
                        Path: $(this).val(),
                        Data: []
                    }
                }
                else if ($(this).hasClass('branchProp')) {
                    branchProperties.push($(this).val().split('/')[1]);
                    tableDic[$(this).val()] = {
                        Text: $(this).text(),
                        Path: $(this).val(),
                        Data: []
                    }
                }
                else if ($(this).hasClass('positionProp')) {
                    positionProperties.push($(this).val().split('/')[1]);
                    tableDic[$(this).val()] = {
                        Text: $(this).text(),
                        Path: $(this).val(),
                        Data: []
                    }
                }
                else if ($(this).hasClass('personalCumulativeProp')) {
                    personalCumulativeProperties.push($(this).val().split('/')[1]);
                }
                else {
                    personalProperties.push($(this).val());
                    tableDic[$(this).val()] = {
                        Text: $(this).text(),
                        Path: $(this).val(),
                        Data: []
                    }
                }
            });
            
            let personalDetailsSelectQuery = personalDetailsProperties.length > 0 ? `PersonalDetails($select=${personalDetailsProperties.join(',')})` : ``;
            let branchSelectQuery = branchProperties.length > 0 ? `Branch($select=${branchProperties.join(',')})` : ``;
            let positionSelectQuery = positionProperties.length > 0 ? `Position($select=${positionProperties.join(',')})` : ``;
            let personalCumulativeSelectQuery = personalCumulativeProperties.length > 0 ? `PersonalCumulatives($select=Year,${personalCumulativeProperties.join(',')})` : ``;
            
            let expandQueries = [];
            if (personalDetailsSelectQuery) expandQueries.push(personalDetailsSelectQuery);
            if (branchSelectQuery) expandQueries.push(branchSelectQuery);
            if (positionSelectQuery) expandQueries.push(positionSelectQuery);
            if (personalCumulativeSelectQuery) expandQueries.push(personalCumulativeSelectQuery);
            let expandQuery = expandQueries.length > 0 ? `&expand=${expandQueries.join(',')}` : "";
            
            let personalSelectQuery = personalProperties.length > 0 ? `&$select=${personalProperties.join(',')}` : `&$select=ID`
            //Gelen filtreler üzerinde Status durumu ile alakalı bir veri yoksa default olarak online olanları getir
            if (!odataFilters.some(item => /(^|[^\/])Status($|[^\/])/.test(item))) {
                odataFilters.push("(Status eq 'Online')")
            }
            if (!odataFilters.some(item => item.includes("Branch/Status"))) {
                odataFilters.push("(Branch/Status eq 'Online')")
            }
            if (!odataFilters.some(item => item.includes("Position/Status"))) {
                odataFilters.push("(Position/Status eq 'Online')")
            }
            let odataQuery = `${odataFilters.length > 0 ? `$filter=${odataFilters.join(' and ')}` : ''}${expandQuery}${personalSelectQuery}`;
            console.log(odataQuery);
            const postData = {
                "filter": odataFilters.length > 0 ? `${odataFilters.join(' and ')}` : "",
                "orderby": "",
                "expand": expandQueries.length > 0 ? `${expandQueries.join(',')}` : "",
                "select": personalProperties.length > 0 ? `${personalProperties.join(',')}` : ""
            };
            $.ajax({
                type: "POST",
                url: `/query/detayli-filtre`,
                contentType:"application/json",
                data: JSON.stringify(postData),
                success: function (res) {
                    $('#mainDiv').removeClass('d-none');
                    $('#page-loader').addClass('d-none')
                    if (res.isSuccess && res.data) {
                        // Verilerle tableDic'i doldurun
                        res.data.forEach(row => {
                            for (let key in tableDic) {
                                let path = tableDic[key].Path;
                                let value = findValueByKey(row, path);
                                if (value !== undefined) {
                                    tableDic[key].Data.push(value);
                                }
                            }
                        });
                        createDynamicTable(tableDic, personalCumulativeProperties, res.data);
                    } else {
                        $('#error-modal-message').text(res.message);
                        $('#error-modal').modal('show');
                    }
                },
                error: function () {
                    $('#mainDiv').removeClass('d-none');
                    $('#page-loader').addClass('d-none')
                    $('#error-modal-message').text("İşleminiz sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyinizx!");
                    $('#error-modal').modal('show');
                }
            });
            

        }
    });
    function findValueByKey(obj, path) {
        let keys = path.includes('/') ? path.split('/') : [path];
        let value = obj;
        for (let key of keys) {
            if (value && value.hasOwnProperty(key)) {
                value = value[key];
            } else {
                return undefined;
            }
        }
        return value;
    }
    function createDynamicTable(tableDic, personalCumulativeProperties, data) {
        $('#results').addClass("card table-responsive m-5 mt-3")
        let table = '<table id="dynamicTable" class="table"><thead><tr>';
        let columns = []
        // Başlıkları ekleyin
        for (let key in tableDic) {     
            table += `<th>
                    <button class="table-sort" data-order="default" data-sort="${key}">${tableDic[key].Text}</button>
                    </th>`;
            columns.push(key);
        }
        let maxYears = [];
        if (personalCumulativeProperties.length > 0) {
            // En uzun yıllar listesini bulalım
            data.forEach(person => {
                let personYears = person.PersonalCumulatives.map(c => c.Year);
                if (personYears.length > maxYears.length) {
                    maxYears = personYears;
                }
            });
            maxYears = maxYears.sort((a, b) => b - a)
            maxYears.forEach(year => {
                personalCumulativeProperties.forEach(prop => {
                    prop = prop === "EarnedYearLeave" ? "Hak Edilen" : (prop === "RemainYearLeave" ? "Kalan İzin" : "")
                    table += `<th>
                        <button class="table-sort" data-order="default" data-sort="${year}-${prop}">${year}<br> ${prop}</button>
                        </th>`;
                    columns.push(`${year}-${prop}`);
                });
            });
        }
        table += '</tr></thead><tbody>';

        let numRows = Object.values(tableDic)[0].Data.length;
        for (let i = 0; i < numRows; i++) {
            table += '<tr>';
            for (let key in tableDic) {
                let cellData = tableDic[key].Data[i];
                let cellText = (cellData === null || cellData === undefined)
                    ? "Yok"
                    : (typeof cellData === 'boolean')
                        ? (cellData ? "Evet" : "Hayır")
                        : cellData;
                table += `<td>${cellText}</td>`;
            }
            // Personel Cumulative Değerleri
            if (personalCumulativeProperties.length > 0) {
                let personCumulativeData = data[i].PersonalCumulatives || [];
                maxYears.forEach(year => {
                    personalCumulativeProperties.forEach(prop => {
                        let yearData = personCumulativeData.find(c => c.Year === year);
                        let value = yearData ? yearData[prop] : undefined;
                        table += `<td>${value !== undefined ? value : 'Yok'}</td>`;
                    });
                });
            }
            table += '</tr>';

        }

        table += '</tbody></table>';

        $('#results').html(table);
        // Datatable üzerinde sorting yaparken türkçe karakter sorunu çözümü
        $.fn.dataTable.ext.type.order['turkish-string-pre'] = function (d) {
            return d.toLowerCase()
                .replace(/ç/g, 'c')
                .replace(/ğ/g, 'g')
                .replace(/ı/g, 'i')
                .replace(/ö/g, 'o')
                .replace(/ş/g, 's')
                .replace(/ü/g, 'u');
        };
        //Datatable Section
        let tableElement = $('#dynamicTable').DataTable({
            dom: 'lfBtip',
            buttons: [
                {
                    extend: 'excelHtml5',
                    text: `<svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-file-spreadsheet" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                                    <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
                                    <path d="M14 3v4a1 1 0 0 0 1 1h4"></path>
                                    <path d="M17 21h-10a2 2 0 0 1 -2 -2v-14a2 2 0 0 1 2 -2h7l5 5v11a2 2 0 0 1 -2 2z"></path>
                                    <path d="M8 11h8v7h-8z"></path>
                                    <path d="M8 15h8"></path>
                                    <path d="M11 11v7"></path>
                                </svg>Excel Raporu`,
                    titleAttr: 'Excel',
                    customize: function (xlsx) {
                        var sheet = xlsx.xl.worksheets['sheet1.xml'];

                        // Tüm hücreleri kontrol ederek uygun şekilde düzenle
                        $('row c', sheet).each(function () {
                            var cellValue = $(this).find('v').text(); // Hücre değerini al
                            if (cellValue) { // Eğer değer boş değilse işleme devam et
                                $(this).attr('t', 'inlineStr'); // Hücreyi metin olarak ayarla
                                $(this).html('<is><t>' + cellValue + '</t></is>'); // Değeri koruyarak metin formatında yaz
                            }
                        });
                    }
                }
            ],
            "language": datatableLanguage,
            "paging": true,
            "searching": true,
            "ordering": true,
            "pageLength": 10,
            "lengthMenu": [5, 10, 25, 50, 100],
            "columnDefs": [
                {
                    targets: '_all',
                    type: 'turkish-string', // Custom sort type
                    render: function (data, type, row) {
                        
                        let formats = ["YYYY-MM-DDTHH:mm:ss.SSSSSSS", "YYYY-MM-DDTHH:mm:ss"];
                        let validFormat = formats.find(format => moment(data, format, true).isValid());
                        if (validFormat) {
                            if (type === 'display') {
                                // Display format: '1 Ocak 1974'
                                return moment(data, moment.ISO_8601).format('DD.MM.YYYY');
                            } else if (type === 'sort' || type === 'type') {
                                // Sort format: 'YYYYMMDD'
                                return moment(data, moment.ISO_8601).format('YYYYMMDD');
                            }
                        }
                        return data;
                    },
                },
            ],
            "initComplete": function () {
                let excelButton = $('.dt-buttons button')
                $('#dynamicTable_wrapper').addClass('p-3')
                $('.dataTables_filter input').addClass('form-control');
                $('.dataTables_filter').append(excelButton)
                excelButton.removeClass().addClass('btn btn-success ms-2')
                new TomSelect($('.dataTables_length select'))
                let dataTableTopContainer = $('<div class="d-flex justify-content-between mb-3"></div>');
                $('.dataTables_length').appendTo(dataTableTopContainer);
                $('.dataTables_filter').appendTo(dataTableTopContainer);
                dataTableTopContainer.insertBefore('#dynamicTable');
                let dataTableBottomContainer = $('<div class="d-flex justify-content-between mt-3"></div>');
                $('.dataTables_info').appendTo(dataTableBottomContainer);
                $('.dataTables_paginate').appendTo(dataTableBottomContainer);
                dataTableBottomContainer.insertAfter('#dynamicTable');
            },


        });
        
        // Sıralama butonları için olay dinleyici ekleme
        $('button.table-sort').on('click', function () {
            let sortColumn = $(this).data('sort');
            let order = $(this).hasClass('asc') ? 'asc' : 'desc';

            // Tüm butonları default hale getir
            $('button.table-sort.asc').removeClass('asc');
            $('button.table-sort.desc').removeClass('desc');
            // Şu anki butonu gizle ve sıralama yapılan butonu göster
            if (order === 'desc') {
                $(`button.table-sort[data-sort="${sortColumn}"]`).addClass('asc').removeClass('desc');
            } else if (order === 'asc') {
                $(`button.table-sort[data-sort="${sortColumn}"]`).addClass('desc').removeClass('asc');
            } else {
                $(`button.table-sort[data-sort="${sortColumn}"]`).addClass('asc').removeClass('desc');
            }
            //// DataTables ile sıralama işlemini gerçekleştirme
            tableElement.order([columns.indexOf(sortColumn), order]).draw();
        });
    }
});