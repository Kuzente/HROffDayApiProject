
//Selectleri Tom Select Yap
document.querySelectorAll('.selectListTom').forEach((el) => {
    let settings = {
        plugins: ['dropdown_input']
    };
    let select = new TomSelect(el, settings);
    
    if (el.getAttribute('data-departmantSelect') !== null) {
        const departmantsJson = `
        [
             {
    "Name": "Yönetim",
    "Value": "Yönetim"
  },
   {
    "Name": "Satın Alma Departmanı",
    "Value": "Satın Alma Departmanı"
  },
   {
    "Name": "Muhasebe Servisi",
    "Value": "Muhasebe Servisi"
  },
   {
    "Name": "Genel Müdür",
    "Value": "Genel Müdür"
  },
   {
    "Name": "Bilgi İşlem Servisi",
    "Value": "Bilgi İşlem Servisi"
  },
   {
    "Name": "Finans Müdürü",
    "Value": "Finans Müdürü"
  },
   {
    "Name": "Santral Görevlisi",
    "Value": "Santral Görevlisi"
  },
    

  {
    "Name": "Depo",
    "Value": "Depo"
  },
  {
    "Name": "Ortaklar Servisi",
    "Value": "Ortaklar Servisi"
  },
  {
    "Name": "Otel",
    "Value": "Otel"
  },
  {
    "Name": "Vezne",
    "Value": "Vezne"
  },
  {
    "Name": "Çay Servisi",
    "Value": "Çay Servisi"
  },
  {
    "Name": "İnsan Kaynakları Departmanı",
    "Value": "İnsan Kaynakları Departmanı"
  },
  {
    "Name": "E-Ticaret",
    "Value": "E-Ticaret"
  },
  {
    "Name": "Basın ve Reklam Birimi",
    "Value": "Basın ve Reklam Birimi"
  },
  {
    "Name": "Kasap",
    "Value": "Kasap"
  },
  {
    "Name": "Şarküteri",
    "Value": "Şarküteri"
  },
  {
    "Name": "Süpermarket Sorumlusu",
    "Value": "Süpermarket Sorumlusu"
  },
  {
    "Name": "Kasiyer",
    "Value": "Kasiyer"
  },
  {
    "Name": "Ara Eleman",
    "Value": "Ara Eleman"
  },
  {
    "Name": "Kozmetik",
    "Value": "Kozmetik"
  },
  {
    "Name": "E-Ticaret Ürün Hazırlama",
    "Value": "E-Ticaret Ürün Hazırlama"
  },
  {
    "Name": "İnşaat / Teknik",
    "Value": "İnşaat / Teknik"
  },
  {
    "Name": "Kuruyemiş",
    "Value": "Kuruyemiş"
  },
  {
    "Name": "İş Takibi",
    "Value": "İş Takibi"
  },
  {
    "Name": "Araba Toplama (Sepetçi)",
    "Value": "Araba Toplama (Sepetçi)"
  },
  {
    "Name": "Personel Yemekhanesi",
    "Value": "Personel Yemekhanesi"
  },
  {
    "Name": "Kısmi Süreli",
    "Value": "Kısmi Süreli"
  },
  {
    "Name": "Avm Market Şubesi",
    "Value": "Avm Market Şubesi"
  },
  {
    "Name": "Ayazmana Şubesi",
    "Value": "Ayazmana Şubesi"
  },
  {
    "Name": "Bulvar Şubesi",
    "Value": "Bulvar Şubesi"
  },
  {
    "Name": "Burdur Şubesi",
    "Value": "Burdur Şubesi"
  },
  {
    "Name": "Çünür Şubesi",
    "Value": "Çünür Şubesi"
  },
  {
    "Name": "Dinar Şubesi",
    "Value": "Dinar Şubesi"
  },
  {
    "Name": "Gölcük Şubesi",
    "Value": "Gölcük Şubesi"
  },
  {
    "Name": "Halıkent Şubesi",
    "Value": "Halıkent Şubesi"
  },
  {
    "Name": "Iyaş Park İdari",
    "Value": "Iyaş Park İdari"
  },
  {
    "Name": "Kesikbaş Şubesi",
    "Value": "Kesikbaş Şubesi"
  },
  {
    "Name": "Kongre Merkezi",
    "Value": "Kongre Merkezi"
  },
  {
    "Name": "Nokta Şubesi",
    "Value": "Nokta Şubesi"
  },
  {
    "Name": "Akaryakıt (Shell)",
    "Value": "Akaryakıt (Shell)"
  },
  {
    "Name": "Part-Time",
    "Value": "Part-Time"
  }
        ]
        `
        const departmants = JSON.parse(departmantsJson);
        departmants.forEach((departmant) => {
            select.addOption({
                value: departmant.Value,
                text: departmant.Value
            });
        });
    }
    if(el.getAttribute('data-reasonSelect')!== null){
        const reasonJson = `
        [
            {
              "Name": "İstirahat (01)",
              "Value": "İstirahat (01)"
            },
            {
              "Name": "Disiplin Cezası (03)",
              "Value": "Disiplin Cezası (03)"
            },
            {
              "Name": "Gözaltına Alınma (04)",
              "Value": "Gözaltına Alınma (04)"
            },
            {
              "Name": "Tutukluluk (05)",
              "Value": "Tutukluluk (05)"
            },
            {
              "Name": "Kısmi İstihdam (06)",
              "Value": "Kısmi İstihdam (06)"
            },
            {
              "Name": "Puantaj Kayıtları (07)",
              "Value": "Puantaj Kayıtları (07)"
            },
            {
              "Name": "Grev (08)",
              "Value": "Grev (08)"
            },
            {
              "Name": "Lokavt (09)",
              "Value": "Lokavt (09)"
            },
            {
              "Name": "Genel Hayatı Etkileyen Olaylar (10)",
              "Value": "Genel Hayatı Etkileyen Olaylar (10)"
            },
            {
              "Name": "Doğal Afet (11)",
              "Value": "Doğal Afet (11)"
            },
            {
              "Name": "Birden Fazla (12)",
              "Value": "Birden Fazla (12)"
            },
            {
              "Name": "Diğer Nedenler (13)",
              "Value": "Diğer Nedenler (13)"
            },
            {
              "Name": "Devamsızlık (15)",
              "Value": "Devamsızlık (15)"
            },
            {
              "Name": "Fesih tarihinde çalışmamış(16)",
              "Value": "Fesih tarihinde çalışmamış (16)"
            },
            {
              "Name": "Ev hizmetlerinde 30 günden az çalışma (17)",
              "Value": "Ev hizmetlerinde 30 günden az çalışma (17)"
            },
            {
              "Name": "Kısa çalışma ödeneği (18)",
              "Value": "Kısa çalışma ödeneği (18)"
            },
            {
              "Name": "Ücretsiz Doğum İzni (19)",
              "Value": "Ücretsiz Doğum İzni (19)"
            },
            {
              "Name": "Ücretsiz Yol İzni (20)",
              "Value": "Ücretsiz Yol İzni (20)"
            },
            {
              "Name": "5434 SK. ek 76, GM 192 (22)",
              "Value": "5434 SK. ek 76, GM 192 (22)"
            },
            {
              "Name": "Yarım çalışma ödeneği (23)",
              "Value": "Yarım çalışma ödeneği (23)"
            },
            {
              "Name": "Yarım çalışma ödeneği ve diğer nedenler (24)",
              "Value": "Yarım çalışma ödeneği ve diğer nedenler (24)"
            },
            {
              "Name": "Diğer belge/kanun türlerinden gün tamamlama (25)",
              "Value": "Diğer belge/kanun türlerinden gün tamamlama (25)"
            },
            {
              "Name": "Kısmi istihdama izin verilen yabancı uyruklu sigortalı (26)",
              "Value": "Kısmi istihdama izin verilen yabancı uyruklu sigortalı (26)"
            },
            {
              "Name": "Kısa Çalışma Ödeneği ve Diğer Nedenler (27)",
              "Value": "Kısa Çalışma Ödeneği ve Diğer Nedenler (27)"
            },
            {
              "Name": "Pandemi Ücretsiz İzin (4857 Geç. 10 Md.) (28)",
              "Value": "Pandemi Ücretsiz İzin (4857 Geç. 10 Md.) (28)"
            },
            {
              "Name": "Pandemi Ücretsiz İzin (4857 Geç. 10.Md) Ve Diğer (29)",
              "Value": "Pandemi Ücretsiz İzin (4857 Geç. 10.Md) Ve Diğer (29)"
            }
        ]
        `
        const reasons = JSON.parse(reasonJson);
        reasons.forEach((reason) => {
            select.addOption({
                value: reason.Value,
                text: reason.Value
            }) 
        })
    }
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
function spinnerStart(element) {
    element.addClass("disabled").append(" <span class=\"spinner-border spinner-border-sm ms-2\" role=\"status\"></span>")
}
function spinnerEnd(element) {
    element.removeClass("disabled").find("span.spinner-border").remove();
}
