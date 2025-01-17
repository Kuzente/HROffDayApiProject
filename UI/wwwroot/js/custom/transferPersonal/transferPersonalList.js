﻿document.addEventListener('DOMContentLoaded', function () {
    let selectMonth = new TomSelect($('select[name="filterMonth"]'));
    let selectYear = new TomSelect($('select[name="filterYear"]'));
    let searchParams = new URLSearchParams(window.location.search);
    //Arama Kısmı metod
    if (searchParams.has("search")) {
        $('#searchInput').val(searchParams.get('search'))
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
    
    if (searchParams.has("filterMonth")) {
        selectMonth.setValue(searchParams.get('filterMonth'))
    }
    if (searchParams.has("filterYear")) {
        selectYear.setValue(searchParams.get('filterYear'))
    }
    $('button[data-sort]').click(function() {
        let $btn = $(this)
        let sortValue = $btn.data('sort').split('-')[1];
        let sortOrder = 'asc';
        if ($btn.hasClass('asc')) {
            sortOrder = 'desc';
        }
        searchParams.set('sortName', sortValue);
        searchParams.set('sortBy', sortOrder);
        window.location.href = window.location.pathname + '?' + searchParams.toString()
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
        if (pageParam) {
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
    $('#deleteForm').submit(function (event) {
        event.preventDefault(); // Formun normal submit işlemini engelle
        spinnerStart($('#deleteButton'))
        let formData = $(this).serializeArray(); // Form verilerini al
        $.ajax({
            type: "POST",
            url: `/personel-nakil-sil`,
            data: formData
        }).done(function (res) {
            spinnerEnd($('#deleteButton'))
            if (res.isSuccess) {
                $('#success-modal-message').text("Kayıt Başarılı Bir Şekilde Silindi.")
                $('#success-modal').modal('show')
                $('#success-modal').on('hidden.bs.modal', function () {
                    window.location.reload();
                });
            }
            else {
                $('#error-modal-message').text(res.message)
                $('#error-modal').modal('show')
            }
        })
    });
    $('#transferListExcelButton').on('click', function (e) {
        e.preventDefault();
        $("#branchExcelForm").submit();
    });
})