﻿document.addEventListener('DOMContentLoaded',function () {
    let filterDateInput = $('.date-range');
    let checkFreeDay = $('#filterForm input[name="isFreedayLeave"]')
    //filtre alanları
    let searchParams = new URLSearchParams(window.location.search);
    if (searchParams.has("filterDate")) {
        // query üzerinden gelen tarihleri ayrıştırıyoruz
        const queryDates = searchParams.get("filterDate").split(" ile ");
        //flatpickr instanceını değişkene atıyoruz
        const flatpickrInstance = filterDateInput[0]._flatpickr;
        //ilgili tarihleri değişkene set ediyoruz
        flatpickrInstance.setDate(queryDates);
    }
    if (searchParams.has("isFreedayLeave")) {
        checkFreeDay.prop('checked', true)
    }
    //Arama Yapılınca çalışan metod
    if (searchParams.has("search")){
        $('#searchInput').val(searchParams.get('search'))
    }
    //Sorting
    if (!searchParams.has('sortName') || !searchParams.has('sortBy') || searchParams.get('sortBy') === '') {
        $('button[data-sort="sort-createdAt"]').addClass('desc');
    } else {
        if (searchParams.get('sortBy') === 'asc') {
            $('button[data-sort="sort-' + searchParams.get('sortName') + '"]').addClass('asc');
        } else {
            $('button[data-sort="sort-' + searchParams.get('sortName') + '"]').addClass('desc');
        }
    }
    //Sorting
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
    // Sil butonuna tıklanınca
    $('[data-deleteButton]').on('click',function () {
        $('#itemIdInput').val($(this).data("item-id"));
        $('#personalNamePlaceholder').text($(this).data("item-personal"));
    });
    //Modal üzerideki İzni İptal Et Butonuna Tıklandığında
    $('#deleteOffDayForm').submit(function (e) {
        e.preventDefault();
        spinnerStart($('#deleteOffdayButton'))
        let formData = $("#deleteOffDayForm").serializeArray();
        $.ajax({
            type: "POST",
            url: "/izin-sil",
            data: formData // Form verilerini al
        }).done(function (res) {
            spinnerEnd($('#deleteOffdayButton'))
            if (res.isSuccess){
                $('#success-modal-message').text("İzin Başarılı Bir Şekilde İptal Edildi.")
                $('#success-modal').modal('show')
                $('#success-modal').on('hidden.bs.modal', function () {
                    window.location.reload();
                });
            }
            else{
                $('#error-modal-message').text(res.message)
                $('#error-modal').modal('show')
            }
        })
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
});