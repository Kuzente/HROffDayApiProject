﻿document.addEventListener('DOMContentLoaded',function () {
    let filterDateInput = $('.date-range');
    setFilterOptions();
    function setFilterOptions() {
        let urlParams = new URLSearchParams(window.location.search);

        if (urlParams.has("filterDate")) {
            // query üzerinden gelen tarihleri ayrıştırıyoruz
            const queryDates = searchParams.get("filterDate").split(" ile ");
            //flatpickr instanceını değişkene atıyoruz
            const flatpickrInstance = filterDateInput[0]._flatpickr;
            //ilgili tarihleri değişkene set ediyoruz
            flatpickrInstance.setDate(queryDates);
        }
       
        if (urlParams.has("search")){
            $('#searchInput').val(urlParams.get('search'))
        }
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
    //İzni Onayla butonuna tıklandığında calısan metod
    $('[data-approveButton]').on('click',function (e) {
        $('#mainDiv').addClass('d-none');
        $('#page-loader').removeClass('d-none')
        e.preventDefault();
        let actionForm = $(this).closest('form[data-approveForm]').attr("action")
        let formData = $(this).closest('form[data-approveForm]').serializeArray();
        $.ajax({
            type: "POST",
            url: actionForm,
            data: formData // Form verilerini al
        }).done(function (res) {
            if (res.isSuccess){
                $('#success-modal-message').text("İzin Onaylandı.")
                $('#success-modal').modal('show')
                $('#success-modal-button').click(function () {
                    window.location.reload();
                });
            }
            else{
                $('#mainDiv').removeClass('d-none');
                $('#page-loader').addClass('d-none')
                $('#error-modal-message').text(res.message)
                $('#error-modal').modal('show')
            }
        })
    });
    //İzni Reddet butonuna tıklandığında calısan metod
    $('[data-rejectButton]').on('click',function (e) {
        $('#mainDiv').addClass('d-none');
        $('#page-loader').removeClass('d-none')
        e.preventDefault();
        let actionForm = $(this).closest('form[data-rejectForm]').attr("action")
        let formData = $(this).closest('form[data-rejectForm]').serializeArray();
        $.ajax({
            type: "POST",
            url: actionForm,
            data: formData // Form verilerini al
        }).done(function (res) {
            if (res.isSuccess){
                $('#success-modal-message').text("İzin Reddedildi.")
                $('#success-modal').modal('show')
                $('#success-modal-button').click(function () {
                    window.location.reload();
                });
            }
            else{
                $('#mainDiv').removeClass('d-none');
                $('#page-loader').addClass('d-none')
                $('#error-modal-message').text(res.message)
                $('#error-modal').modal('show')
            }
        })
    });
});