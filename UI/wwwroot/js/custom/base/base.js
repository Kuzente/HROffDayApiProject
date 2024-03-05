//Selectleri Tom Select Yap
document.querySelectorAll('.selectListTom').forEach((el) => {
    let settings = {
        plugins: ['dropdown_input']
    };
    new TomSelect(el, settings);

});
// Tüm date inputlarını seçin
let dateInputs = document.querySelectorAll('input[type="date"]');
//Flatpickr ile bootstrap cakısma onleyici yıl secimi yapılamıyordu
document.addEventListener('focusin', (e) => {
    if (e.target.closest(".flatpickr-calendar") !== null) {
        e.stopImmediatePropagation();
    }
});
// Her bir date input için Flatpicker'ı başlatın
dateInputs.forEach(function (input) {
    flatpickr(input, {
        altInput: true,
        altFormat: "d F Y",
        dateFormat: "Y-m-d",
        locale: {
            weekdays: {
                longhand: ['Pazar', 'Pazartesi', 'Salı', 'Çarşamba', 'Perşembe', 'Cuma', 'Cumartesi'],
                shorthand: ['Paz', 'Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt']
            },
            months: {
                longhand: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'],
                shorthand: ['Oca', 'Şub', 'Mar', 'Nis', 'May', 'Haz', 'Tem', 'Ağu', 'Eyl', 'Eki', 'Kas', 'Ara']
            },
            today: 'Bugün',
            clear: 'Temizle'
        }
    });
});
let currentPage = window.location.pathname;
$("[data-sidebarListItem]").each(function() {
    let link = $(this).find("a").attr("href");
    if ($(this).closest(".dropdown-menu").length > 0) {
        if (currentPage === link) {
            $(this).addClass("active");
            $(this).closest(".dropdown-menu").toggle()
        }

    } else {
        if (currentPage === link) {
            $(this).addClass("active");
        }  
    }
   
});
