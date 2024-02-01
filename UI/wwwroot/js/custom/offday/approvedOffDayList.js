document.addEventListener('DOMContentLoaded',function () {
    let selectYear = new TomSelect($("#filterYear"));
    let selectMonth = new TomSelect($("#filterMonth"));
    let selectBranch;
    let selectPosition;
    
    //Filtre Ayarları
    function setFilterOptions() {
        let urlParams = new URLSearchParams(window.location.search);
        let filterYear = urlParams.get('filterYear');
        let filterMonth = urlParams.get('filterMonth');
        let filterPosition = urlParams.get('positionName');
        if (filterYear) {
            selectYear.setValue([filterYear]);
        }
        if (filterMonth) {
            
            selectMonth.setValue([filterMonth]);
        }
        $.ajax({ //TODO
            type: "GET",
            url : "/get-select-items"
        }).done(function (res) {
            $('#branchSelect').empty();
            $('#positionSelect').empty();
            $.each(res.branches, function (index , branch) {
                $('#branchSelect').append(`<option value='${branch.name}'>${branch.name}</option>`);
            });
            $.each(res.positions, function (index , position) {
                $('#positionSelect').append(`<option value='${position.name}'>${position.name}</option>`);
            });
            selectBranch = new TomSelect($("#branchSelect"));
            selectPosition = new TomSelect($("#positionSelect"));
            selectPosition.clear();
            selectBranch.clear();
        });
        
       
    }
    setFilterOptions();
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
});