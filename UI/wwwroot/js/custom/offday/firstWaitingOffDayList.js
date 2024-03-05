document.addEventListener('DOMContentLoaded',function () {
    let selectYear = new TomSelect($("[name='filterYear']"));
    let selectMonth = new TomSelect($("[name='filterMonth']"));
    setFilterOptions();
    function setFilterOptions() {
        let urlParams = new URLSearchParams(window.location.search);
        let filterYear = urlParams.get('filterYear');
        let filterMonth = urlParams.get('filterMonth');
        if (filterYear) {
            selectYear.setValue([filterYear]);
        }
        if (filterMonth) {
            selectMonth.setValue([filterMonth]);
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
                $('#error-modal-message').text(res.message)
                $('#error-modal').modal('show')
            }
        })
    });
    //İzni Reddet butonuna tıklandığında calısan metod
    $('[data-rejectButton]').on('click',function (e) {
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
                $('#error-modal-message').text(res.message)
                $('#error-modal').modal('show')
            }
        })
    });
});