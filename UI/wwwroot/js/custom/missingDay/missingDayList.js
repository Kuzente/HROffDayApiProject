document.addEventListener("DOMContentLoaded", () => {
    let filterDateInput = $('.date-range');
    let filterReasonSelect = new TomSelect($("#filterReason"), { maxOptions: null });
    let addModalReasonInput = new TomSelect($("#addModalReasonInput"), { maxOptions: null });
    let filterBranchSelect = new TomSelect($("#filterBranch"), { maxOptions: null });
    let addModalBranchInput = new TomSelect($("#addModalBranchInput"), { maxOptions: null });
    let searchParams = new URLSearchParams(window.location.search);
    $.ajax({
        type: "GET",
        url: "json/missingDay-reasons.json",
        dataType: "json",
        success: (data) => {
            data.forEach((item) => {
                filterReasonSelect.addOption({
                    value: item.Value,
                    text: item.Value
                });
                addModalReasonInput.addOption({
                    value: item.Value,
                    text: item.Value
                });
            });
            if (searchParams.has("filterReason")) {
                filterReasonSelect.setValue(searchParams.get("filterReason"));
            }
        },
        error: () => {
            $('#error-modal-message').text("Şubelere ait liste verileri getirilirken bir hata oluştu!");
            $('#error-modal').modal('show');
        }
    });
    $.ajax({
        type: "GET",
        url: "/query/tum-subeler?$select=Name,ID&$orderby=Name asc&$filter=Status eq 'Online'",
        dataType: "json",
        success: (data) => {
            data.forEach((item) => {
                filterBranchSelect.addOption({
                    value: item.ID,
                    text: item.Name
                });
                addModalBranchInput.addOption({
                    value: item.ID,
                    text: item.Name
                });
            });
            if (searchParams.has("filterBranch")) {
                filterBranchSelect.setValue(searchParams.get("filterBranch"));
            }
        },
        error: () => {
            $('#error-modal-message').text("Şubelere ait liste verileri getirilirken bir hata oluştu!");
            $('#error-modal').modal('show');
        }
    });
    if (searchParams.has("search")) {
        $('#searchInput').val(searchParams.get('search'));
    }
    if (searchParams.has("filterDate")) {
        // query üzerinden gelen tarihleri ayrıştırıyoruz
        const queryDates = searchParams.get("filterDate").split(" ile ");
        //flatpickr instanceını değişkene atıyoruz
        const flatpickrInstance = filterDateInput[0]._flatpickr;
        //ilgili tarihleri değişkene set ediyoruz
        flatpickrInstance.setDate(queryDates);
    }
    if (!searchParams.has('sortName') || !searchParams.has('sortBy') || searchParams.get('sortBy') === '') {
        $('button[data-sort="sort-createdAt"]').addClass('desc');
    } else {
        if (searchParams.get('sortBy') === 'asc') {
            $('button[data-sort="sort-' + searchParams.get('sortName') + '"]').addClass('asc');
        } else {
            $('button[data-sort="sort-' + searchParams.get('sortName') + '"]').addClass('desc');
        }
    }

    $('button[data-sort]').click(function () {
        let $btn = $(this);
        let sortValue = $btn.data('sort').split('-')[1];
        let sortOrder = 'asc';
        if ($btn.hasClass('asc')) {
            sortOrder = 'desc';
        }
        searchParams.set('sortName', sortValue);
        searchParams.set('sortBy', sortOrder);
        window.location.href = window.location.pathname + '?' + searchParams.toString();
    });
    $('[data-firstButton]').on('click', function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        // Eğer "sayfa" parametresi varsa, değerini 1 azalt
        if (pageParam) {
            currentUrl.searchParams.set("sayfa", 1);
        }
        window.location.href = currentUrl.toString();
    });
    $('[data-prevButton]').on('click', function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        // Eğer "sayfa" parametresi varsa, değerini 1 azalt
        if (pageParam && pageParam > 0) {
            let newPage = parseInt(pageParam) - 1;
            currentUrl.searchParams.set("sayfa", newPage);
        }
        // Yeni URL'e yönlendir
        window.location.href = currentUrl.toString();
    });
    $('[data-nextButton]').on('click', function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        // Eğer "sayfa" parametresi varsa, değerini 1 azalt
        if (pageParam) {
            let newPage = parseInt(pageParam) + 1;
            currentUrl.searchParams.set("sayfa", newPage);
        }
        else
            currentUrl.searchParams.set("sayfa", 2);

        window.location.href = currentUrl.toString();
    });
    $('[data-lastButton]').on('click', function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        currentUrl.searchParams.set("sayfa", this.getAttribute('data-lastButton'));

        window.location.href = currentUrl.toString();
    });
    $('[data-paginationButton]').on('click', function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        currentUrl.searchParams.set("sayfa", $(this).text());

        window.location.href = currentUrl.toString();
    });
    // Sil butonuna tıklanınca
    $('[data-deleteButton]').on('click', function () {
        $('#itemIdInput').val($(this).data("item-id"));
        $('#personalNamePlaceholder').text($(this).data("item-personal"));
    });
    //Sil Post
    $("#deleteForm").submit(function (event) {
        event.preventDefault(); // Formun normal submit işlemini engelle
        spinnerStart($("#deleteButton"));
        const formData = $(this).serializeArray();// Form verilerini al
        const actionUrl = $(this).attr('action');// Form action al
        $.ajax({
            type: "POST",
            url: actionUrl,
            data: formData
        }).done((res) => {
            spinnerEnd($("#deleteButton"));
            if (res.isSuccess) {
                $("#success-modal-message").text("Kayıt Başarılı Bir Şekilde Silindi.");
                $("#success-modal").modal("show");
                $("#success-modal").on("hidden.bs.modal", () => {
                    window.location.reload();
                });
            } else {
                $("#error-modal-message").text(res.message);
                $("#error-modal").modal("show");
            }
        });
    });
    $('#missingDayExcelButton').on('click', function (e) {
        e.preventDefault();
        $("#missingDayExcelForm").submit();
    });
    $('#addModal').on('hidden.bs.modal', function (e) {
        // Form alanınızı resetleme
        $("#addMissingDayForm")[0].reset();
        $("#addModalReasonInput")[0].tomselect.clear();
        $("#addModalBranchInput")[0].tomselect.clear();
    });
    addModalBranchInput.on("change", (selectedBranchId) => {
            if (selectedBranchId) {
                $('#addModalPersonalSelectRow').empty();
                const personnelSelect = 
                ` <div class="col mb-3">
                    <label class="form-label required">Personel Şubesi</label>
                   <select id="addModalPersonalInput" data-required="true" class="form-select" name="PersonalId" placeholder="Personel Seçiniz"></select>
                    
                   </div>
                    `;
                $("#addModalPersonalSelectRow").append(personnelSelect);
                const personnelTomSelect = new TomSelect($("#addModalPersonalInput"), { maxOptions: null });
                $.ajax({
                    type: "GET",
                    url: `/query/detayli-filtre/Personal?$select=NameSurname,ID,Branch_Id&$orderby=NameSurname asc&$filter=Status eq 'Online'and Branch_Id eq ${selectedBranchId}`, // Personel verilerini almak için uygun URL
                    dataType: "json",
                    success: (data) => {
                         //Personel select'ini doldur
                        data.forEach((item) => {
                            personnelTomSelect.addOption({
                                value: item.ID,
                                text: item.NameSurname
                            });
                        });
                        
                    },
                    error: () => {
                        $('#error-modal-message').text("Personel verileri getirilirken bir hata oluştu!");
                        $('#error-modal').modal('show');
                    }
                });
            }
            else {
                $('#addModalPersonalSelectRow').empty(); 
            }
    });
    function checkRequiredFields() {
        let isValid = true;
        $("[data-required='true']").each(function () {
            if ($(this).val() === "") {
                isValid = false;
                return false;
            }
        });
        return isValid;
    }
    $('#addMissingDayForm').submit(function (e) {
        e.preventDefault();
        if (!checkRequiredFields()) {
            $('#error-modal-message').text("Lütfen Zorunlu Alanları Girdiğinizden Emin Olunuz.");
            $('#error-modal').modal('show');
            return; // Fonksiyondan çık
        }
        else {
            spinnerStart($('#addMissingDayButton'));
            const formData = $("#addMissingDayForm").serializeArray();
            const formAction = $(this).attr('action');
            console.log(formData);
            $.ajax({
                type: "POST",
                url: formAction,
                data: formData // Form verilerini al
            }).done(function(res) {
                spinnerEnd($('#addMissingDayButton'));
                if (res.isSuccess) {
                    $('#success-modal-message').text("Eksik Gün Başarılı Bir Şekilde Eklendi.");
                    $('#success-modal').modal('show');
                    $('#success-modal').on('hidden.bs.modal', () => window.location.reload());
                } else {
                    $('#error-modal-message').text(res.message);
                    $('#error-modal').modal('show');
                }
            });
        }

    });


});