document.addEventListener('DOMContentLoaded', function () {

    let BranchListDirector = [];
    let BranchListBranchManager = [];
    let searchParams = new URLSearchParams(window.location.search);
    //Arama Kısmı metod
    if (searchParams.has("search")){
        $('#searchInput').val(searchParams.get('search'))
    }
    //Filtrele Kısmı Metod
    if(searchParams.has("isActive")){
        let activeInput = $('#filterForm').find('input[value="active"]')
        let passiveInput = $('#filterForm').find('input[value="passive"]')
        searchParams.get('isActive') === "active" ? activeInput.prop('checked', true) : passiveInput.prop('checked', true);
    }
    if (!searchParams.has('sortName') || !searchParams.has('sortBy') || searchParams.get('sortBy') === '') {
        $('button[data-sort="sort-branchName"]').addClass('asc');
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
    //Checkboxlar üzerinde dinleme metodu
    document.querySelectorAll('[name="role"]').forEach(function(radio) {
        radio.addEventListener('change', function() {
            if (this.value === '1') {
                createBranchSelectElement(true); // multiple için true
                
            } else if(this.value === '2') {
                createBranchSelectElement(false); // multiple için false
            }
            else{
                // Genel Müdür dışındaki seçenekler seçildiğinde şube seçimi alanını kaldır
                let divSection = document.querySelector('#branchSection >.row > .col.mb-3');
                if (divSection) {
                    divSection.remove();
                }
            }
        });
    });
    $.ajax({
        type: "GET",
        url: "/select-director-branch",
    }).done(function (res) {
        if (res.isSuccess){
            BranchListDirector = res.data;
            if (res.data.length > 0){
                $('#alertRow').append(`<div class="alert alert-important alert-danger alert-dismissible" role="alert">
            <div class="d-flex">
                <div>
                    <!-- Download SVG icon from http://tabler-icons.io/i/alert-circle -->
                    <svg xmlns="http://www.w3.org/2000/svg" class="icon alert-icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                        <path stroke="none" d="M0 0h24v24H0z" fill="none"></path><path d="M3 12a9 9 0 1 0 18 0a9 9 0 0 0 -18 0"></path><path d="M12 8v4"></path><path d="M12 16h.01"></path>
                    </svg>
                </div>
                <div>
                    Genel Müdür Atanmamış Şubeler Mevcut (${BranchListDirector.map(p => p.name).join(', ')})
                </div>
            </div>
        </div>`)
            }
        }
        else{
            $('#error-modal-message').text("Lütfen Sisteme Şube Ekleyiniz!")
            $('#error-modal').modal('show')
        }
    });
    $.ajax({
        type: "GET",
        url: "/select-branchmanager-branch",
    }).done(function (res) {
        if (res.isSuccess){
            BranchListBranchManager = res.data;
            if (res.data.length > 0){
                $('#alertRow').append(`<div class="alert alert-important alert-danger alert-dismissible" role="alert">
            <div class="d-flex">
                <div>
                    <!-- Download SVG icon from http://tabler-icons.io/i/alert-circle -->
                    <svg xmlns="http://www.w3.org/2000/svg" class="icon alert-icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                        <path stroke="none" d="M0 0h24v24H0z" fill="none"></path><path d="M3 12a9 9 0 1 0 18 0a9 9 0 0 0 -18 0"></path><path d="M12 8v4"></path><path d="M12 16h.01"></path>
                    </svg>
                </div>
                <div>
                    Şube Sorumlusu Atanmamış Şubeler Mevcut (${BranchListBranchManager.map(p => p.name).join(', ')})
                </div>
            </div>
        </div>`)
            }
        }
        else{
            $('#error-modal-message').text("Lütfen Sisteme Şube Ekleyiniz!")
            $('#error-modal').modal('show')
        }
    });
    //Select Elementleri Yaratma Fonksiyonu
    function createBranchSelectElement(multiple) {
        let divSectionClear = document.querySelector('#branchSection >.row > .col.mb-3');
        if (divSectionClear) {
            divSectionClear.remove();
        }
        let branchSelectDiv = document.createElement('div');
        branchSelectDiv.classList.add('row');
        branchSelectDiv.innerHTML = `
        <div class="col mb-3">
            <label for="branchSelect" class="form-label required">Şube Seçiniz</label>
            <select name="BranchNames" ${multiple ? 'multiple' : ''} id="branchSelect" class="form-select" placeholder="Şube Seçiniz!">
            </select>
        </div>
    `;
        let divSection = document.getElementById('branchSection');
        divSection.appendChild(branchSelectDiv);
        if(multiple){
            BranchListDirector.forEach(function(branch) {
                let option = document.createElement('option');
                option.value = branch.id;
                option.text = branch.name;
                $('#branchSelect').append(option);
            });
        }
        else{
            BranchListBranchManager.forEach(function(branch) {
                let option = document.createElement('option');
                option.value = branch.id;
                option.text = branch.name;
                $('#branchSelect').append(option);
            });  
        }
        new TomSelect($('#branchSelect'));
    };
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
    function isValidEmail(email) {
        // Düzenli ifade kullanarak mail formatını kontrol et
        let re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }
   
    //Formu Resetleme
    $('#addModal').on('hidden.bs.modal', function (e) {
        // Form alanınızı resetleme
        $('#addUserForm')[0].reset();
        let divSection = document.querySelector('#branchSection >.row > .col.mb-3');
        if (divSection) {
            divSection.remove();
        }
        $('#hrInput').prop('checked', true);
        $('#branchManagerInput').prop('checked', false);
        $('#directorInput').prop('checked', false);
    });
    //Kullanıcı Ekle Butonu basıldığında
    $('#addUserButton').click(function () {
        let email = document.getElementById("mailInput").value;
        spinnerStart($('#addUserButton'))
        if(!checkRequiredFields()){
            spinnerEnd($('#addUserButton'))
            $('#error-modal-message').text("Lütfen Zorunlu Alanları Girdiğinizden Emin Olunuz.")
            $('#error-modal').modal('show')
            return;
        }
        if (!isValidEmail(email)) {
            spinnerEnd($('#addUserButton'))
            $('#error-modal-message').text("Lütfen Geçerli Bir Mail Adresi Girdiğinizden Emin Olunuz.")
            $('#error-modal').modal('show')
            return;
        }
        let formData = $("#addUserForm").serializeArray();
        $.ajax({
            type: "POST",
            url: "/kullanici-ekle",
            data : formData
        }).done(function (res) {
            spinnerEnd($('#addUserButton'))
            if (res.isSuccess){
                $('#success-modal-message').text("Kullanıcı Başarılı Bir Şekilde Eklendi.")
                $('#success-modal').modal('show')
                $('#success-modal-button').click(function () {
                    window.location.reload();
                });
            }
            else{
                $('#error-modal-message').text(res.message)
                $('#error-modal').modal('show')
            }
        });
    })
    // Sil butonuna tıklanınca
    $('[data-deleteButton]').on('click',function () {
        $('#itemIdInput').val($(this).data("item-id"));
        $('#personalNamePlaceholder').text($(this).data("item-personal"));
    });
    //Modal üzerideki Şubeyi Sil Butonuna Tıklandığında
    $('#deleteUserButton').on('click',function (e) {
        spinnerStart($('#deleteUserButton'))
        e.preventDefault();
        let formData = $("#deleteUserForm").serializeArray();
        $.ajax({
            type: "POST",
            url: "/kullanici-sil",
            data: formData // Form verilerini al
        }).done(function (res) {
            spinnerEnd($('#deleteUserButton'))
            if (res.isSuccess){
                $('#success-modal-message').text("Kullanıcı Başarılı Bir Şekilde Silindi.")
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