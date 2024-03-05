document.addEventListener('DOMContentLoaded',function () {
    let selectYear = new TomSelect($("#filterYear"));
    let selectMonth = new TomSelect($("#filterMonth"));
    let currentUrl = new URL(window.location.href);
    setFilterOptions();
    setPersonalHeader();
    
    //Filtre Ayarları
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
    function setPersonalHeader() {
        let personal_id = currentUrl.searchParams.get("id");
        $.ajax({
           type:"POST",
           url:`/personel-header?id=${personal_id}` 
        }).done(function (res) {
            if (res.isSuccess){
                $('#HeaderPersonalNameSurname').text(res.data.nameSurname);
                $('#HeaderPersonalBranchPosition').text(res.data.branch.name + " - " + res.data.position.name);
                if (res.data.status === 0) { // Online
                    $('#headerButton').addClass("btn-secondary").removeClass("btn-orange");
                    $('#headerButton span').html("İşten Çıkar");
                } else {
                    $('#headerButton').addClass("btn-orange").removeClass("btn-secondary");
                    $('#headerButton span').html("İşe Geri Al");
                }
                $('#personalAvatar').html(res.data.nameSurname.charAt(0));
                
                $('#badgeTotalTakenLeave').html(saatleriGunVeSaatlereCevir(res.data.totalTakenLeave));
                $('#badgeTotalYearLeave').html(res.data.totalYearLeave);
                $('#badgeUsedYearLeave').html(res.data.usedYearLeave);
                $('#balanceYearLeave').html(res.data.totalYearLeave - res.data.usedYearLeave);
                
            }
            else{
                $('#error-modal-message').text(res.message)
                $('#error-modal').modal('show');
                $('#error-modal-button').click(function () {
                    window.location.href = "/Personal";
                })
            }
        });
    }
    function saatleriGunVeSaatlereCevir(saat) {
        // Toplam saatleri gün ve saatlere çevir
        let gun = Math.floor(saat / 8);
        let kalanSaat = saat % 8;

        // Sonucu döndür
        return gun + " gün " + kalanSaat + " saat";
    }
    // Sil butonuna tıklanınca
    $('[data-deleteButton]').on('click',function () {
        $(this).data("item-personal" , $('#HeaderPersonalNameSurname').text());
        $('#itemIdInput').val($(this).data("item-id"));
        $('#personalNamePlaceholder').text($(this).data("item-personal"));
    });
    $('#headerButton').on("click", function () {
        $.ajax({
            type: "POST",
            url: `/personel-durumu?id=${currentUrl.searchParams.get("id")}`
        }).done(function (res) {
            if (res.isSuccess){
                console.log("Başarılı");
            }else{
                console.log("Başarısız");
            }
        }).then(function () {
            window.location = window.location;
        });
    });
    $('[data-firstButton]').on('click',function () {
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