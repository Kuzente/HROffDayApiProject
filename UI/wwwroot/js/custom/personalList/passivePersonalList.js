document.addEventListener('DOMContentLoaded',function () {
    let searchParams = new URLSearchParams(window.location.search);
    if (searchParams.has("search")){
        $('#searchInput').val(searchParams.get('search'))
    }

    if (!searchParams.has('sortName') || !searchParams.has('sortBy') || searchParams.get('sortBy') === '') {
        $('button[data-sort="sort-nameSurname"]').addClass('asc');
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
    //Filtrele Kısmı Metod
    if(searchParams.has("gender")){
        let erkekInput = $('#filterForm').find('input[value="erkek"]')
        let kadinInput = $('#filterForm').find('input[value="kadın"]')
        searchParams.get('gender') === "erkek" ? erkekInput.prop('checked', true) : kadinInput.prop('checked', true);
    }
    if(searchParams.has("retired")){
        let retiredInput = $('#filterForm').find('input[name="retired"]')
        searchParams.get('retired') === "true" ? retiredInput.prop('checked', true) : "";
    }
    $.ajax({ //TODO
        type: "GET",
        url : "/get-select-items"
    }).done(function (res) {
        $('#positionSelect').empty();
        $('#branchSelect').empty();
        $.each(res.branches, function (index , branch) {
            $('#branchSelect').append(`<option value='${branch.name}'>${branch.name}</option>`);
        });
        $.each(res.positions, function (index , position) {
            $('#positionSelect').append(`<option value='${position.name}'>${position.name}</option>`);
        });
        let branchSelect = new TomSelect($("#branchSelect"));
        let positionSelect = new TomSelect($("#positionSelect"));
        positionSelect.clear();
        branchSelect.clear();
        //Filtre
        if (searchParams.has('position')){
            positionSelect.setValue([searchParams.get('position')])
        }
        if (searchParams.has('branch')){
            branchSelect.setValue([searchParams.get('branch')])
        }
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