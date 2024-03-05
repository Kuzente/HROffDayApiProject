﻿document.addEventListener('DOMContentLoaded',function () {
    $('#addModal').on('hidden.bs.modal', function (e) {
        // Form alanınızı resetleme
        $('#addBranchForm')[0].reset();
    });
    // Sil butonuna tıklanınca
    $('[data-deleteButton]').on('click',function () {
        $('#itemIdInput').val($(this).data("item-id"));
        $('#personalNamePlaceholder').text($(this).data("item-personal"));
    });
    
    $('#branchExcelButton').on('click',function (e) {
        e.preventDefault();
        $("#branchExcelForm").submit();
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
    $('#addBranchButton').on('click',function (e) {
        e.preventDefault();
        let formData = $("#addBranchForm").serializeArray();
        $.ajax({
            type: "POST",
            url: "/sube-ekle",
            data: formData // Form verilerini al
        }).done(function (res) {
            if (res.isSuccess){
                $('#success-modal-message').text("Şube Başarılı Bir Şekilde Eklendi.")
                $('#success-modal').modal('show')
                $('#success-modal-button').click(function () {
                   window.location.href = "/subeler"
                });
            }
            else{
                $('#error-modal-message').text(res.message)
                $('#error-modal').modal('show')
            }
        })
    });
    //Modal üzerideki Şubeyi Sil Butonuna Tıklandığında
    $('#deleteBranchButton').on('click',function (e) {
        e.preventDefault();
        let formData = $("#deleteBranchForm").serializeArray();
        $.ajax({
            type: "POST",
            url: "/sube-sil",
            data: formData // Form verilerini al
        }).done(function (res) {
            if (res.isSuccess){
                $('#success-modal-message').text("Şube Başarılı Bir Şekilde Silindi.")
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