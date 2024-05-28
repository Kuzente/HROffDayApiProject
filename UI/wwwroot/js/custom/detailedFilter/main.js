document.addEventListener('DOMContentLoaded',function () {
    //<--!Static Data Section -->
    let bloodGroupList;
    let bodySize;
    let educationStatus;
    let maritalStatus;
    let personalGroup;
    let departments;
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
        let selectedEntityName = $(this).find('option:selected').text()
        let selectedEntityValue = $(this).find('option:selected').val()
        const addFilterBtn = $('#addFilterBtn')
        const sendFilterBtn = $('#sendFiltersBtn')
        let filtersRowSection = $('#filtersRowSection')
        filtersRowSection.empty()
        addFilterBtn.addClass('d-none')
        sendFilterBtn.addClass('d-none')
        if (selectedEntityValue){
            addFilterBtn.removeClass('d-none')
            sendFilterBtn.removeClass('d-none')
        }
    });
    
    
    //Filtre Ekle butonuna ait function
    $('#addFilterBtn').click(function() {
        let selectedEntityVal = $('#entitySelect').find('option:selected').val()
        let newRow;
        let selectId = "select_" + generateGUID()
        if (selectedEntityVal === 'Personal'){
            newRow = `
                    <div class="row mt-3 filter-row">
                        <div class="col d-flex align-items-end">
                            <button class="btn btn-danger btn-icon remove-filter">x</button>
                            <div class="ms-2 d-inline-block">
                                <label class="form-label">Filtre Uygulamak İstediğiniz Kolonu Seçiniz</label>
                                <select class="form-select propertySelect" placeholder="Kolon Seçiniz!!!" name="propertyName" id="${selectId}">
                                    <option value=""></option>
                                    <optgroup label="Metin">
                                        <option data-type="Text" value="NameSurname">Adı-Soyadı</option>
                                        <option data-type="Text" value="BirthPlace">Doğum Yeri</option>
                                        <option data-type="Text" value="IdentificationNumber">Tc Kimlik Numarası</option>
                                        <option data-type="Text" value="SskNumber">SSK Numarası</option>
                                        <option data-type="Text" value="SgkCode">SGK Kodu</option>
                                        <option data-type="Text" value="Phonenumber">Telefon</option>
                                        <option data-type="Text" value="FatherName">Baba Adı</option>
                                        <option data-type="Text" value="MotherName">Anne Adı</option>
                                        <option data-type="Text" value="IBAN">IBAN</option>
                                        <option data-type="Text" value="BankAccount">Banka Hesap No</option>
                                        <option data-type="Text" value="Address">Adres</option>
                                    </optgroup>
                                    <optgroup label="Liste">
                                        <option data-type="List" value="Gender">Cinsiyet</option>
                                        <option data-type="List" value="Branch">Şube</option>
                                        <option data-type="List" value="Position">Ünvan / Görev</option>
                                        <option data-type="List" value="DepartmantName">Departman</option>
                                        <option data-type="List" value="BloodGroup">Kan Grubu</option>
                                        <option data-type="List" value="MaritalStatus">Medeni Durumu</option>
                                        <option data-type="List" value="EducationStatus">Eğitim Durumu</option>
                                        <option data-type="List" value="PersonalGroup">Personelin Bulunduğu Grup</option>
                                        <option data-type="List" value="BodySize">Beden Ölçüsü</option>
                                    </optgroup>
                                    <optgroup label="Sayı">
                                        <option data-type="Number" value="TotalYearLeave">Hak Edilen Yıllık İzin Miktarı</option>
                                        <option data-type="Number" value="UsedYearLeave">Kullanılan Yıllık İzin Miktarı</option>
                                        <option data-type="Number" value="RegistirationNumber">Sicil Numarası</option>
                                        <option data-type="Number" value="FoodAid">Gıda Yardımı Miktarı</option>
                                    </optgroup>
                                    <optgroup label="Ondalıklı Sayı">
                                        <option data-type="Double" value="TotalTakenLeave">Mevcut Alacak İzin Miktarı</option>
                                        <option data-type="Double" value="Salary">Maaş</option>
                                    </optgroup>
                                    <optgroup label="Tarih">
                                        <option data-type="Date" value="BirthDate">Doğum Tarihi</option>
                                        <option data-type="Date" value="StartJobDate">İşe Başlama Tarihi</option>
                                        <option data-type="Date" value="EndJobDate">İşten Çıkış Tarihi</option>                                  
                                        <option data-type="Date" value="YearLeaveDate">Yıllık İzin Yenilenme Tarihi</option>
                                        <option data-type="Date" value="RetiredDate">Emeklilik Tarihi</option>
                                        <option data-type="Date" value="FoodAidDate">Gıda Yardımı Yenilenme Tarihi</option>
                                    </optgroup>
                                    <optgroup label="Seçimli">
                                        <option data-type="Radio" value="RetiredOrOld">Emeklilik Durumu</option>
                                        <option data-type="Radio" value="IsYearLeaveRetired">Yıllık İzini Emeklilik Durumu İle Yenilenenler</option>
                                        <option data-type="Radio" value="Handicapped">Engellilik Durumu</option>
                                        <option data-type="Radio" value="IsBackToWork">İşe Geri Alınma Durumu</option>
                                    </optgroup>  
                                </select>
                            </div>
                            <div class="ms-5 d-inline-block border-start border-3 border-dark ps-4 filtersDetailsSection">
                            
                            </div>
                        </div>
                    </div>`; 
        }
        $('#filtersRowSection').append(newRow);
        let select = new TomSelect($(`#${selectId}`),{
            maxOptions : null,
        });
        select.clear()
        
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
                <input type="text" name="${selectedPropValue}_search" class="form-control" placeholder="Boş Bırakılırsa boş değer arayacaktır!">
            `;
            filtersDetailsSection.append(input)
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
            switch (selectedPropValue) {
                case "Gender":
                    printArray = [{Name: "Erkek", Value: "Erkek"}, {Name: "Kadın", Value: "Kadın"}]
                    break;
                case "BloodGroup":
                    printArray = bloodGroupList
                    break;
                case "MaritalStatus" :
                    printArray = maritalStatus
                    break;
                case "PersonalGroup" :
                    printArray = personalGroup
                    break;
                case "EducationStatus" :
                    printArray = educationStatus
                    break;
                case "BodySize" :
                    printArray = bodySize
                    break;
                case "DepartmantName" :
                    printArray = departments
                    break;
            }
            $.each(printArray, function (index, item) {
                selectTom.addOption({
                    value: item.Value,
                    text: item.Name
                });
            });
            
        }
        else if (selectedType === 'Number'){
            let comparisonSelect = `
                <div class="d-inline-block">
                    <label class="form-label">${selectedPropName} göre filtre uygulayınız.</label>
                    <select class="form-select numberSelect" placeholder="Karşılaştırma Seçin" name="comparisonType">
                             <option data-propname="${selectedPropValue}" value=""></option>
                             <option data-propname="${selectedPropValue}" value="equal">Eşit</option>
                             <option data-propname="${selectedPropValue}" value="notEqual">Eşit Değil</option>
                             <option data-propname="${selectedPropValue}" value="greater">Büyük</option>
                             <option data-propname="${selectedPropValue}" value="greaterThanEqual">Büyük Eşit</option>
                             <option data-propname="${selectedPropValue}" value="less">Küçük</option>
                             <option data-propname="${selectedPropValue}" value="lessThanEqual">Küçük Eşit</option>
                             <option data-propname="${selectedPropValue}" value="between">Arasında</option>
                    </select> 
                </div>
            `;
            filtersDetailsSection.append(comparisonSelect)
            new TomSelect(filtersDetailsSection.find('select'))
        }
        else if (selectedType === 'Double'){
            let comparisonSelect = `
                <div class="d-inline-block">
                    <label class="form-label">${selectedPropName} göre filtre uygulayınız.</label>
                    <select class="form-select doubleSelect" placeholder="Karşılaştırma Seçin" name="comparisonType">
                             <option data-propname="${selectedPropValue}" value=""></option>
                             <option data-propname="${selectedPropValue}" value="equal">Eşit</option>
                             <option data-propname="${selectedPropValue}" value="notEqual">Eşit Değil</option>
                             <option data-propname="${selectedPropValue}" value="greater">Büyük</option>
                             <option data-propname="${selectedPropValue}" value="greaterThanEqual">Büyük Eşit</option>
                             <option data-propname="${selectedPropValue}" value="less">Küçük</option>
                             <option data-propname="${selectedPropValue}" value="lessThanEqual">Küçük Eşit</option>
                             <option data-propname="${selectedPropValue}" value="between">Arasında</option>
                    </select> 
                </div>
            `;
            filtersDetailsSection.append(comparisonSelect)
            new TomSelect(filtersDetailsSection.find('select'))
        }
        else if (selectedType === 'Date'){
            let comparisonSelect = `
                <div class="d-inline-block">
                    <label class="form-label">${selectedPropName} göre filtre uygulayınız.</label>
                    <select class="form-select dateSelect" placeholder="Karşılaştırma Seçin" name="comparisonType">
                             <option data-propname="${selectedPropValue}" value=""></option>
                             <option data-propname="${selectedPropValue}" value="equal">Eşit</option>
                             <option data-propname="${selectedPropValue}" value="notEqual">Eşit Değil</option>
                             <option data-propname="${selectedPropValue}" value="greater">Büyük</option>
                             <option data-propname="${selectedPropValue}" value="greaterThanEqual">Büyük Eşit</option>
                             <option data-propname="${selectedPropValue}" value="less">Küçük</option>
                             <option data-propname="${selectedPropValue}" value="lessThanEqual">Küçük Eşit</option>
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
                    <input type="radio" name="${selectedPropValue}_radio" value="True" class="form-check-input" id="Emekli${selectGuid}">
                    <label class="form-check-label" for="Emekli${selectGuid}">Emekli</label>
                </div>
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="False" class="form-check-input" id="Normal${selectGuid}">
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
                    <input type="radio" name="${selectedPropValue}_radio" value="True" class="form-check-input" id="True${selectGuid}">
                    <label class="form-check-label" for="True${selectGuid}">Emeklilik ile yenilenen</label>
                </div>
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="False" class="form-check-input" id="False${selectGuid}">
                    <label class="form-check-label" for="False${selectGuid}">Normal yenilenen</label>
                </div>
                </div>
            `;
                filtersDetailsSection.append(radios)
            }
            else if (selectedPropValue === 'Handicapped'){
                let radios = `
                <label class="form-label">${selectedPropName} seçimi yapınız.</label>
                <div class="row">
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="True" class="form-check-input" id="True${selectGuid}">
                    <label class="form-check-label" for="True${selectGuid}">Engelli</label>
                </div>
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="False" class="form-check-input" id="False${selectGuid}">
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
                    <input type="radio" name="${selectedPropValue}_radio" value="True" class="form-check-input" id="True${selectGuid}">
                    <label class="form-check-label" for="True${selectGuid}">İşe Geri Alınan</label>
                </div>
                <div class="form-check col-6">
                    <input type="radio" name="${selectedPropValue}_radio" value="False" class="form-check-input" id="False${selectGuid}">
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
    $(document).on('change', '.numberSelect', function() {
        let selectedValue = $(this).find('option:selected').val();
        let filtersDetailsSection = $(this).closest(".row").find('.filtersDetailsSection')
        let comparisonSection = $(this).closest(".row").find('.comparisonSection')
        comparisonSection.remove()
        if(selectedValue === 'between'){
            
            let input1 = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input type="number" name="Amount" class="form-control" placeholder="Miktar 1">
            </div>
            `;
            let input2 = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input type="number" name="Amount" class="form-control" placeholder="Miktar 2">
            </div>
            `;
            filtersDetailsSection.append(input1)
            filtersDetailsSection.append(input2)
        }
        else{
            let input = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input type="number" name="Amount" class="form-control" placeholder="Miktar">
            </div>
            `;
            filtersDetailsSection.append(input)
        }
    });
    //Ondalıklı Sayı seçimlerinde karşılaştırma selecti değişme functionu
    $(document).on('change', '.doubleSelect', function() {
        let selectedValue = $(this).find('option:selected').val();
        let filtersDetailsSection = $(this).closest(".row").find('.filtersDetailsSection')
        let comparisonSection = $(this).closest(".row").find('.comparisonSection')
        comparisonSection.remove()
        if(selectedValue === 'between'){

            let input1 = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input type="number" name="Amount" class="form-control input-double" placeholder="Miktar 1">
            </div>
            `;
            let input2 = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input type="number" name="Amount" class="form-control input-double" placeholder="Miktar 2">
            </div>
            `;
            filtersDetailsSection.append(input1)
            filtersDetailsSection.append(input2)
        }
        else{
            let input = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input type="number" name="Amount" class="form-control input-double" placeholder="Miktar">
            </div>
            `;
            filtersDetailsSection.append(input)
        }
    });
    //Date seçimlerinde karşılaştırma selecti değişme functionu
    $(document).on('change', '.dateSelect', function() {
        let selectedValue = $(this).find('option:selected').val();
        let filtersDetailsSection = $(this).closest(".row").find('.filtersDetailsSection')
        let comparisonSection = $(this).closest(".row").find('.comparisonSection')
        comparisonSection.remove()
        if(selectedValue === 'between'){
            let input1 = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input type="date" name="Date" class="form-control input-date" placeholder="Tarih 1">
            </div>
            `;
            let input2 = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input type="date" name="Date" class="form-control input-date" placeholder="Tarih 2">
            </div>
            `;
            filtersDetailsSection.append(input1)
            filtersDetailsSection.append(input2)
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
            })
        }
        else{
            let input = `
            <div class="ms-2 d-inline-block comparisonSection">
                <input type="date" name="Date" class="form-control input-date" placeholder="Tarih">
            </div>
            `;
            filtersDetailsSection.append(input)
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
            })
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
    $('#sendFiltersBtn').click(function() {
        let isFormValid = true;
        let tableName = $('select[name="entityName"]').find('option:selected').val()
        if (tableName === ""){
            $('#error-modal-message').text("Lütfen tablo seçtiğinizden emin olunuz!")
            $('#error-modal').modal('show')
            isFormValid = false
            return false;
        }
        let filters = [];
        $('#filtersRowSection .filter-row').each(function() {
            let propertySelect = $(this).find('.propertySelect');
            let selectedType = propertySelect.find('option:selected').data('type');
            let selectedPropValue = propertySelect.val();
            let selectedPropName = propertySelect.find('option:selected').text();
            if (!selectedType){
                $('#error-modal-message').text(`Lütfen kolon seçtiğinizden emin olunuz!`)
                $('#error-modal').modal('show')
                isFormValid = false;
                return  false;
            } 
            else if (selectedType === 'Text') {
                let searchValue = $(this).find(`input[name="${selectedPropValue}_search"]`).val();
                console.log(searchValue)
                filters.push({
                    "Property": selectedPropValue,
                    "Type": selectedType,
                    "SearchValue": searchValue
                });
            }
            else if (selectedType === "List"){
                let selectedValues = [];
                $(this).find('select[name="ListSelect"] option:selected').each(function() {
                    selectedValues.push($(this).val());
                });
                filters.push({
                    "Property": selectedPropValue,
                    "Type": selectedType,
                    "SelectedValues": selectedValues
                })
            }
            else if (selectedType === 'Number') {
                let comparisonType = $(this).find('.numberSelect').val();
                if (!comparisonType){
                    $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                    $('#error-modal').modal('show')
                    isFormValid = false;
                    return false;
                }
                else if (comparisonType === 'between') {
                    let amounts = $(this).find('input[name="Amount"]');
                    let amount1 = $(amounts[0]).val();
                    let amount2 = $(amounts[1]).val();
                    if (!amount1 || !amount2){
                        $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                        $('#error-modal').modal('show')
                        isFormValid = false;
                        return false;  
                    }
                    filters.push({
                        "Property": selectedPropValue,
                        "Type": selectedType,
                        "Comparison": comparisonType,
                        "Amounts": [
                            parseInt(amount1),
                            parseInt(amount2)
                        ],
                    });
                } else {
                    let amount = $(this).find('input[name="Amount"]').val();
                    if (!amount){
                        $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                        $('#error-modal').modal('show')
                        isFormValid = false;
                        return false;
                    }
                    filters.push({
                        "Property": selectedPropValue,
                        "Type": selectedType,
                        "Comparison": comparisonType,
                        "Amounts": [parseInt(amount)]
                    });
                }
            }
            else if (selectedType === 'Double') {
                let comparisonType = $(this).find('.doubleSelect').val();
                if (!comparisonType){
                    $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                    $('#error-modal').modal('show')
                    isFormValid = false;
                    return false;
                }
                else if (comparisonType === 'between') {
                    let amounts = $(this).find('input[name="Amount"]');
                    let amount1 = $(amounts[0]).val();
                    let amount2 = $(amounts[1]).val();
                    if (!amount1 || !amount2){
                        $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                        $('#error-modal').modal('show')
                        isFormValid = false;
                        return false;
                    }
                    filters.push({
                        "Property": selectedPropValue,
                        "Type": selectedType,
                        "Comparison": comparisonType,
                        "Amounts": [
                            parseFloat(amount1),
                            parseFloat(amount2)
                        ],
                    });
                } else {
                    let amount = $(this).find('input[name="Amount"]').val();
                    if (!amount){
                        $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                        $('#error-modal').modal('show')
                        isFormValid = false;
                        return false;
                    }
                    filters.push({
                        "Property": selectedPropValue,
                        "Type": selectedType,
                        "Comparison": comparisonType,
                        "Amounts": [parseFloat(amount)]
                    });
                }
            }
            else if (selectedType === 'Date') {
                let comparisonType = $(this).find('.dateSelect').val();
                if (!comparisonType){
                    $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                    $('#error-modal').modal('show')
                    isFormValid = false;
                    return false;
                }
                else if (comparisonType === 'between') {
                    let dates = $(this).find('input[name="Date"]');
                    let date1 = $(dates[0]).val();
                    let date2 = $(dates[1]).val();
                    if (!date1 || !date2){
                        $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                        $('#error-modal').modal('show')
                        isFormValid = false;
                        return false;
                    }
                    filters.push({
                        "Property": selectedPropValue,
                        "Type": selectedType,
                        "Comparison": comparisonType,
                        "Dates": [
                            date1,
                            date2
                        ],
                    });
                } else {
                    let date = $(this).find('input[name="Date"]').val();
                    if (!date){
                        $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin boş olmadığından emin olunuz!`)
                        $('#error-modal').modal('show')
                        isFormValid = false;
                        return false;
                    }
                    filters.push({
                        "Property": selectedPropValue,
                        "Type": selectedType,
                        "Comparison": comparisonType,
                        "Dates": [date]
                    });
                }
            } 
            else if(selectedType === 'Radio') {
                let checkedValue = $(this).find('input[type="radio"]:checked').val()
                if (!checkedValue){
                    $('#error-modal-message').text(`Lütfen ${selectedPropName} filtresinin seçeneklerinin seçili olduğundan emin olunuz!`)
                    $('#error-modal').modal('show')
                    isFormValid = false;
                    return false;
                }else{
                    filters.push({
                        "Property": selectedPropValue,
                        "Type": selectedType,
                        "CheckedValue": checkedValue
                    });  
                }
            }
        })

        if (isFormValid){
            //let jsonData = JSON.stringify({ "Filters": filters , "EntityName" : tableName });
            //console.log(jsonData); // JSON verisini konsola yazdır
            console.log(filters)
            $.ajax({
                type: "POST",
                url : "/detayli-filtre-sonuc",
                //contentType: "application/json",
                data: {
                    EntityName: tableName,
                    Filters: filters
                }
            }).done(function (res) {
                console.log(res)
            })
        }
    });
});