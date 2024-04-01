
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
    "Name": "Acil Tıp Teknisyeni",
    "Value": "Acil Tıp Teknisyeni"
  },
  {
    "Name": "Akaryakıt İstasyon Sorumlusu",
    "Value": "Akaryakıt İstasyon Sorumlusu"
  },
  {
    "Name": "Akaryakıt Satış Elemanı (Pompacı)",
    "Value": "Akaryakıt Satış Elemanı (Pompacı)"
  },
  {
    "Name": "Aşçı",
    "Value": "Aşçı"
  },
  {
    "Name": "Aşçı Yardımcısı",
    "Value": "Aşçı Yardımcısı"
  },
  {
    "Name": "Bilgi İşlem Destek Elemanı",
    "Value": "Bilgi İşlem Destek Elemanı"
  },
  {
    "Name": "Bilgi İşlem Destek Uzmanı",
    "Value": "Bilgi İşlem Destek Uzmanı"
  },
  {
    "Name": "Bilişim Teknolojileri Teknisyeni",
    "Value": "Bilişim Teknolojileri Teknisyeni"
  },
  {
    "Name": "Bulaşıkçı (Stevard)",
    "Value": "Bulaşıkçı (Stevard)"
  },
  {
    "Name": "Büro Memuru (Genel)",
    "Value": "Büro Memuru (Genel)"
  },
  {
    "Name": "Büro Memuru (İdari İşler)",
    "Value": "Büro Memuru (İdari İşler)"
  },
  {
    "Name": "Büro Yönetimi Elemanı",
    "Value": "Büro Yönetimi Elemanı"
  },
  {
    "Name": "Çaycı - büro, otel ve diğer işyerlerinde",
    "Value": "Çaycı - büro, otel ve diğer işyerlerinde"
  },
  {
    "Name": "Depo Forklift Operatörü",
    "Value": "Depo Forklift Operatörü"
  },
  {
    "Name": "Depo Görevlisi (Gıda)",
    "Value": "Depo Görevlisi (Gıda)"
  },
  {
    "Name": "Depo Lojistik Elemanı",
    "Value": "Depo Lojistik Elemanı"
  },
  {
    "Name": "Depo Sevkiyat Sorumlusu",
    "Value": "Depo Sevkiyat Sorumlusu"
  },
  {
    "Name": "Depo Şoför-Yük Taşıma",
    "Value": "Depo Şoför-Yük Taşıma"
  },
  {
    "Name": "Depo Sorumlusu",
    "Value": "Depo Sorumlusu"
  },
  {
    "Name": "Et Ve Et Ürünleri İşlemecisi",
    "Value": "Et Ve Et Ürünleri İşlemecisi"
  },
  {
    "Name": "Et Ve Et Ürünleri Satış Elemanı",
    "Value": "Et Ve Et Ürünleri Satış Elemanı"
  },
  {
    "Name": "E-Ticaret Meslek Elemanı",
    "Value": "E-Ticaret Meslek Elemanı"
  },
  {
    "Name": "Finansman Yöneticisi",
    "Value": "Finansman Yöneticisi"
  },
  {
    "Name": "Garson (Servis Elemanı)",
    "Value": "Garson (Servis Elemanı)"
  },
  {
    "Name": "Genel Alan Temizleme Görevlisi/Meydancı",
    "Value": "Genel Alan Temizleme Görevlisi/Meydancı"
  },
  {
    "Name": "Genel Müdür",
    "Value": "Genel Müdür"
  },
  {
    "Name": "Genel Müdür Yardımcısı",
    "Value": "Genel Müdür Yardımcısı"
  },
  {
    "Name": "Genel Müdür-Perakende Ve Toptan Ticaret (Özel Sektör)",
    "Value": "Genel Müdür-Perakende Ve Toptan Ticaret (Özel Sektör)"
  },
  {
    "Name": "Grafik Tasarımcısı",
    "Value": "Grafik Tasarımcısı"
  },
  {
    "Name": "Güneş Isıl Sistem Personeli/Güneş Enerjisi Sistemleri Montaj İşçisi",
    "Value": "Güneş Isıl Sistem Personeli/Güneş Enerjisi Sistemleri Montaj İşçisi"
  },
  {
    "Name": "Güneş Isıl Sistem Personeli/Güneş Enerjisi Sistemleri Montajcısı",
    "Value": "Güneş Isıl Sistem Personeli/Güneş Enerjisi Sistemleri Montajcısı"
  },
  {
    "Name": "Halkla İlişkiler Görevlisi",
    "Value": "Halkla İlişkiler Görevlisi"
  },
  {
    "Name": "İnşaat Bekçisi",
    "Value": "İnşaat Bekçisi"
  },
  {
    "Name": "İnsan Kaynakları Uzmanı",
    "Value": "İnsan Kaynakları Uzmanı"
  },
  {
    "Name": "İnsan Kaynakları Yönetimi Meslek Elemanı",
    "Value": "İnsan Kaynakları Yönetimi Meslek Elemanı"
  },
  {
    "Name": "Kasap",
    "Value": "Kasap"
  },
  {
    "Name": "Kasiyer",
    "Value": "Kasiyer"
  },
  {
    "Name": "Kozmetik Ürünleri Satış Elemanı",
    "Value": "Kozmetik Ürünleri Satış Elemanı"
  },
  {
    "Name": "Kuruyemiş Satış Elemanı",
    "Value": "Kuruyemiş Satış Elemanı"
  },
  {
    "Name": "Mağaza Sorumlusu/şefi",
    "Value": "Mağaza Sorumlusu/şefi"
  },
  {
    "Name": "Mali Müşavir",
    "Value": "Mali Müşavir"
  },
  {
    "Name": "Market Elemanı",
    "Value": "Market Elemanı"
  },
  {
    "Name": "Muhasebe Yetkilisi Mutemedi",
    "Value": "Muhasebe Yetkilisi Mutemedi"
  },
  {
    "Name": "Muhasebeci",
    "Value": "Muhasebeci"
  },
  {
    "Name": "Mutfak Görevlisi",
    "Value": "Mutfak Görevlisi"
  },
  {
    "Name": "Otel Müdürü",
    "Value": "Otel Müdürü"
  },
  {
    "Name": "Otopark Görevlisi/Vale",
    "Value": "Otopark Görevlisi/Vale"
  },
  {
    "Name": "Part-time",
    "Value": "Part-time"
  },
  {
    "Name": "Pazarlama Uzmanı",
    "Value": "Pazarlama Uzmanı"
  },
  {
    "Name": "Resepsiyon Görevlisi (Ön Büro Elemanı)",
    "Value": "Resepsiyon Görevlisi (Ön Büro Elemanı)"
  },
  {
    "Name": "Reyon Görevlisi",
    "Value": "Reyon Görevlisi"
  },
  {
    "Name": "Reyon Şefi",
    "Value": "Reyon Şefi"
  },
  {
    "Name": "Şarküteri Ürünleri Satış Elemanı",
    "Value": "Şarküteri Ürünleri Satış Elemanı"
  },
  {
    "Name": "Satın Alma Sorumlusu",
    "Value": "Satın Alma Sorumlusu"
  },
  {
    "Name": "Satın Alma Yöneticisi/Müdürü",
    "Value": "Satın Alma Yöneticisi/Müdürü"
  },
  {
    "Name": "Sekreter",
    "Value": "Sekreter"
  },
  {
    "Name": "Süt Ve Süt Ürünleri Satış Elemanı",
    "Value": "Süt Ve Süt Ürünleri Satış Elemanı"
  },
  {
    "Name": "Teknik / Asma Tavan Ustası (Metal)",
    "Value": "Teknik / Asma Tavan Ustası (Metal)"
  },
  {
    "Name": "Teknik / Beden İşçisi (İnşaat)",
    "Value": "Teknik / Beden İşçisi (İnşaat)"
  },
  {
    "Name": "Teknik / Elektrik Tesisatı Ve Pano Montörü Teknisyeni",
    "Value": "Teknik / Elektrik Tesisatı Ve Pano Montörü Teknisyeni"
  },
  {
    "Name": "Teknik / İnşaat Teknolojisi Teknikeri",
    "Value": "Teknik / İnşaat Teknolojisi Teknikeri"
  },
  {
    "Name": "Teknik / Isıtma Ve Sıhhi Tesisatçı",
    "Value": "Teknik / Isıtma Ve Sıhhi Tesisatçı"
  },
  {
    "Name": "Teknik / Kaynak Teknisyeni",
    "Value": "Teknik / Kaynak Teknisyeni"
  },
  {
    "Name": "Teknik / Sıvacı",
    "Value": "Teknik / Sıvacı"
  },
  {
    "Name": "Teknik Elektrikçi (Genel)",
    "Value": "Teknik Elektrikçi (Genel)"
  },
  {
    "Name": "Teknik Müdür",
    "Value": "Teknik Müdür"
  },
  {
    "Name": "Teknik Servis Elemanı",
    "Value": "Teknik Servis Elemanı"
  },
  {
    "Name": "Yemekhane servis işçisi",
    "Value": "Yemekhane servis işçisi"
  },
  {
    "Name": "Yönetici (Bilgi İşlem)",
    "Value": "Yönetici (Bilgi İşlem)"
  },
  {
    "Name": "Yönetim Kurulu Başkanı (Özel Sektör)",
    "Value": "Yönetim Kurulu Başkanı (Özel Sektör)"
  },
  {
    "Name": "Yönetim Kurulu Üyesi (Özel Sektör)",
    "Value": "Yönetim Kurulu Üyesi (Özel Sektör)"
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
