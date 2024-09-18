document.addEventListener('DOMContentLoaded',function () {
    let branchSelectModal;
    let positionSelectModal ;
    let DepartmantsSelect = new TomSelect($("#departmantNameSelect"),{maxOptions : null})
    let PersonalGroupSelect = new TomSelect($("#PersonalGroupSelect"),{maxOptions : null})
    let EducationStatusSelect = new TomSelect($("#EducationStatusSelect"),{maxOptions : null})
    let MaritalStatusSelect = new TomSelect($("#MaritalStatusSelect"),{maxOptions : null});
    let BodySizeSelect = new TomSelect($("#BodySizeSelect"),{maxOptions : null});
    let BloodGroupSelect =   new TomSelect($("#BloodGroupSelect"),{maxOptions : null});
    let searchParams = new URLSearchParams(window.location.search);
    if (searchParams.has("search")){
        $('#searchInput').val(searchParams.get('search'))
    }
        
    if (!searchParams.has('sortName') || !searchParams.has('sortBy') || searchParams.get('sortBy') === '') {
        $('button[data-sort="sort-nameSurname"]').addClass('asc');
    } else {
        if (searchParams.get('sortBy') === 'asc') {
            $('button[data-sort="sort-' + searchParams.get('sortName') + '"]').addClass('asc');
        } else {
            $('button[data-sort="sort-' + searchParams.get('sortName') + '"]').addClass('desc');
        }
    }
    $('button[data-sort]').click(function () {
        let $btn = $(this)
        let sortValue = $btn.data('sort').split('-')[1];
        let sortOrder = 'asc';
        if ($btn.hasClass('asc')) {
            sortOrder = 'desc';
        }
        searchParams.set('sortName', sortValue);
        searchParams.set('sortBy', sortOrder);
        window.location.href = window.location.pathname + '?' + searchParams.toString()
    })
    //Filtrele Kısmı Metod
    if(searchParams.has("gender")){
        let erkekInput = $('#filterForm').find('input[value="erkek"]')
        let kadinInput = $('#filterForm').find('input[value="kadın"]')
        searchParams.get('gender') === "erkek" ? erkekInput.prop('checked', true) : kadinInput.prop('checked', true);
    }
    if(searchParams.has("retired")){
        let retiredInput = $('#filterForm').find('input[name="retired"]')
        searchParams.get('retired') === "true" ? retiredInput.prop('checked', true) : "";
    }
    $('[data-firstButton]').on('click',function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        // Eğer "sayfa" parametresi varsa, değerini 1 azalt
        if (pageParam) {
            currentUrl.searchParams.set("sayfa", 1);
        }
        window.location.href = currentUrl.toString();
    });
    $('[data-prevButton]').on('click',function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        // Eğer "sayfa" parametresi varsa, değerini 1 azalt
        if (pageParam) {
            let newPage = parseInt(pageParam) - 1;
            currentUrl.searchParams.set("sayfa", newPage);
        }
        // Yeni URL'e yönlendir
        window.location.href = currentUrl.toString();
    });
    $('[data-nextButton]').on('click',function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        // Eğer "sayfa" parametresi varsa, değerini 1 azalt
        if (pageParam) {
            let newPage = parseInt(pageParam) + 1 ;
            currentUrl.searchParams.set("sayfa", newPage);
        }
        else
            currentUrl.searchParams.set("sayfa", 2);

        window.location.href = currentUrl.toString();
    });
    $('[data-lastButton]').on('click',function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        currentUrl.searchParams.set("sayfa", this.getAttribute('data-lastButton'));

        window.location.href = currentUrl.toString();
    });
    $('[data-paginationButton]').on('click',function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        currentUrl.searchParams.set("sayfa", $(this).text());
        window.location.href = currentUrl.toString();
    });
    
    
    
    //Sayfa üzerindeki sube ve ünvan selectleri get metodu
    
    $.ajax({ //TODO
        type: "GET",
        url : "/get-select-items"
    }).done(function (res) {
        $('#branchSelectModal').empty();
        $('#positionSelectModal').empty();
        $('#positionSelect').empty();
        $('#branchSelect').empty();
        $.each(res.branches, function (index , branch) {
            $('#branchSelectModal').append(`<option value='${branch.id}'>${branch.name}</option>`);
            $('#branchSelect').append(`<option value='${branch.name}'>${branch.name}</option>`);
        });
        $.each(res.positions, function (index , position) {
            $('#positionSelectModal').append(`<option value='${position.id}'>${position.name}</option>`);
            $('#positionSelect').append(`<option value='${position.name}'>${position.name}</option>`);
        });
        branchSelectModal = new TomSelect($("#branchSelectModal"),{maxOptions : null});
        let branchSelect = new TomSelect($("#branchSelect"),{maxOptions : null});
        positionSelectModal = new TomSelect($("#positionSelectModal"),{maxOptions : null});
        let positionSelect = new TomSelect($("#positionSelect"),{maxOptions : null});
        positionSelectModal.clear();
        positionSelect.clear();
        branchSelectModal.clear();
        branchSelect.clear();
        //Filtre
        if (searchParams.has('position')){
            positionSelect.setValue([searchParams.get('position')])
        }
        if (searchParams.has('branch')){
            branchSelect.setValue([searchParams.get('branch')])
        }
    });
    //Static Selects ajax get metodu
    $.ajax({
        type: "GET",
        url:"json/personal-selects-data.json",
        dataType: "json",
        success: function (data) {
            data.bloodGroup.forEach((item) => {
                BloodGroupSelect.addOption({
                    value: item.Value,
                    text: item.Value
                })
            });
            data.bodySize.forEach((item) => {
                BodySizeSelect.addOption({
                    value: item.Value,
                    text: item.Value
                })
            });
            data.educationStatus.forEach((item) => {
                EducationStatusSelect.addOption({
                    value: item.Value,
                    text: item.Value
                })
            });
            data.maritalStatus.forEach((item) => {
                MaritalStatusSelect.addOption({
                    value: item.Value,
                    text: item.Value
                })
            });
            data.personalGroup.forEach((item) => {
                PersonalGroupSelect.addOption({
                    value: item.Value,
                    text: item.Value
                })
            });
            data.departments.forEach((item) => {
                DepartmantsSelect.addOption({
                    value: item.Value,
                    text: item.Value
                })
            });
        },
        error: function () {
            $('#error-modal-message').text("Personel Detaylarına ait liste verileri getirilirken bir hata oluştu!")
            $('#error-modal').modal('show');
        }
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
    //Emeklilik Seçimi Değiştiğinde çalışan metod
    $('#RetiredOrOldInput').change(function () {
        let emeklilikLabel = $('label[for="RetiredOrOldInput"]');
        if($(this).is(':checked')){
            emeklilikLabel.addClass("required")
            $(this).closest('.d-flex.flex-column').append(`<input class="form-control justify-content-center m-2 mb-4 flatpickr-input" data-required="true" type="date" name="RetiredDate" id="RetiredDateInput" placeholder="Emeklilik Tarihi">`);
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
            let isYearLeaveRetiredElement = `
            <div id="IsYearLeaveRetiredElement" class="mb-3">
                <label class="form-check">
                    <input type="checkbox" class="form-check-input" data-required="false" name="IsYearLeaveRetired">
                    <span class="form-check-label">Yıllık İzin Yenilemelerinde Emeklilik Durumu Baz Alınsın Mı?</span>
                </label>
            </div>
            `
            $(this).closest('.d-flex.flex-column').append(isYearLeaveRetiredElement)
        }
        else{
            emeklilikLabel.removeClass("required")
            let retiredDateInput = document.getElementById('RetiredDateInput');
            let isYearLeaveRetiredElement = document.getElementById('IsYearLeaveRetiredElement');
            let fpInstance = flatpickr(retiredDateInput);
            fpInstance.destroy();
            retiredDateInput.remove();
            isYearLeaveRetiredElement.remove();
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
        DepartmantsSelect.clear();
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
        spinnerStart($("#personalAddButton"))
        e.preventDefault();
        let formData = $("#addPersonalForm").serializeArray();
        formData.forEach(function (f) {
            if (f.value ==="on"){
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
            url: "/create-personal",
            data: formData // Form verilerini al
        }).done(function (res) {
            spinnerEnd($("#personalAddButton"))
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