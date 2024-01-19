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
});