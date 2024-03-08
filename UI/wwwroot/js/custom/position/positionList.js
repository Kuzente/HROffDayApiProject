document.addEventListener('DOMContentLoaded',function () {
    let searchParams = new URLSearchParams(window.location.search);
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
        $('button[data-sort="sort-positionName"]').addClass('asc');
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
    $('#addModal').on('hidden.bs.modal', function (e) {
        // Form alanınızı resetleme
        $('#addPositionForm')[0].reset();
    });
    // Sil butonuna tıklanınca
    $('[data-deleteButton]').on('click',function () {
        $('#itemIdInput').val($(this).data("item-id"));
        $('#personalNamePlaceholder').text($(this).data("item-personal"));
    });
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
    $('#addPositionButton').on('click',function (e) {
        e.preventDefault();
        let formData = $("#addPositionForm").serializeArray();
        $.ajax({
            type: "POST",
            url: "/unvan-ekle",
            data: formData // Form verilerini al
        }).done(function (res) {
            if (res.isSuccess){
                $('#success-modal-message').text("Ünvan Başarılı Bir Şekilde Eklendi.")
                $('#success-modal').modal('show')
                $('#success-modal-button').click(function () {
                    window.location.href = "/unvanlar"
                });
            }
            else{
                $('#error-modal-message').text(res.message)
                $('#error-modal').modal('show')
            }
        })
    });
    //Modal üzerideki Ünvanı Sil Butonuna Tıklandığında
    $('#deletePositionButton').on('click',function (e) {
        e.preventDefault();
        let formData = $("#deletePositionForm").serializeArray();
        console.log(formData)
        $.ajax({
            type: "POST",
            url: "/unvan-sil",
            data: formData // Form verilerini al
        }).done(function (res) {
            if (res.isSuccess){
                $('#success-modal-message').text("Ünvan Başarılı Bir Şekilde Silindi.")
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