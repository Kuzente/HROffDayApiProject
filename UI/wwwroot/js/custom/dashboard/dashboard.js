document.addEventListener('DOMContentLoaded', function () {
    //const spinner = $(".spinner-border");
    let erkekSayisi = 0;
    let kadinSayisi = 0;
    let SalaryCount = 0;
    let FoodAidCount = 0;
    let PersonalCount = 0;
    let educationCounts= {};
    
    const today = new Date();
    const birthList = document.getElementById('birthList');
    const offDayList = document.getElementById('waitingOffDayList')
    const missDayList = document.getElementById('missingDayList')
    let personalResponse ;
    //İş gücü verisi Fonksiyonu
    function WorkPowerTable(response , selectedYear = new Date().getFullYear(),selectedBranches = []) {
        let currentMonth = new Date().getMonth();
        let monthNames = [
            "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
        ];
        $('#workPowerTitle').text(selectedYear + " Yılı İş Gücü Verileri");
        let tableBody = document.querySelector('.table-bordered tbody');
        monthNames.forEach(function (month, index) {
            let startMonthWorking = 0;
            let monthGetPersonel = 0;
            let monthGetPersonelMan = 0;
            let monthGetPersonelWoman = 0;
            let monthBanPersonel = 0;
            let monthBanPersonelMan = 0;
            let monthBanPersonelWoman = 0;
            let endOfMonthPersonel = 0;
            let firstDayOfMonth = new Date(selectedYear, index, 1);
            let row = document.createElement('tr');
            // Eğer Şube seçili ise ona göre filtreleme yap
            if (selectedBranches.length > 0){
                response = response.filter(person=> selectedBranches.includes(person.Branch.ID))
                let branchNamesSet = new Set(response.map(person => person.Branch.Name)); // Şube adlarını alıyoruz
                let branchNames = [...branchNamesSet]; //Tekile düşürmek için
                let title = selectedYear + " Yılı ";
                if (branchNames.length > 0) {
                    title += branchNames.join(", ") + " ";
                }
                title += "İş Gücü Verileri";
                $('#workPowerTitle').text(title);
            }
            if (selectedYear === today.getFullYear() && index <= currentMonth) {
                response.forEach(function (item) {
                    if (item.StartJobDate) { // EĞER İŞE BAŞLAMA TARİHİ VARSA
                        let startJobDate = new Date(item.StartJobDate);
                        let endJobDate = new Date(item.EndJobDate);
                        if (startJobDate < firstDayOfMonth && (item.EndJobDate === null || endJobDate > firstDayOfMonth)) {
                            startMonthWorking++;
                        } 
                        if (startJobDate.getMonth() === index && startJobDate.getFullYear() === selectedYear) {
                            if (item.Gender === "Kadın") {
                                monthGetPersonelWoman++;
                            } 
                            else if (item.Gender === "Erkek"){
                                monthGetPersonelMan++;
                            }
                            monthGetPersonel++;
                        }
                    }
                    if (item.EndJobDate) { // EĞER İŞTEN ÇIKIŞ TARİHİ VARSA
                        let endJobDate = new Date(item.EndJobDate);
                        if (endJobDate.getMonth() === index && endJobDate.getFullYear() === selectedYear) {
                            if (item.Gender === "Kadın") {
                                monthBanPersonelWoman++;
                            }
                            else if (item.Gender === "Erkek"){
                                monthBanPersonelMan++;
                            }
                            monthBanPersonel++;
                        }
                    }
                });
                // Satıra değerleri ekle
                row.innerHTML = `
                         <td>${month}</td>
                         <td>${startMonthWorking}</td>
                         <td>${monthGetPersonelMan > 0 ? `${monthGetPersonelMan} Erkek` : ""}  ${monthGetPersonelWoman > 0 ? `${monthGetPersonelWoman} Kadın` : ""} ${monthGetPersonelMan <= 0 && monthGetPersonelWoman <= 0 ? "Yok" : ""}</td>
                         <td>${monthBanPersonelMan > 0 ? `${monthBanPersonelMan} Erkek` : ""}  ${monthBanPersonelWoman > 0 ? `${monthBanPersonelWoman} Kadın` : ""} ${monthBanPersonelMan <= 0 && monthBanPersonelWoman <= 0 ? "Yok" : ""}</td>
                         <td>${startMonthWorking + monthGetPersonel - monthBanPersonel}</td> 
        `;

                // Tabloya satırı ekle
                tableBody.appendChild(row);
            } else if (selectedYear < today.getFullYear()) {
                response.forEach(function (item) {
                    if (item.StartJobDate) { // EĞER İŞE BAŞLAMA TARİHİ VARSA
                        let startJobDate = new Date(item.StartJobDate);
                        let endJobDate = new Date(item.EndJobDate);
                        if (startJobDate < firstDayOfMonth && (item.EndJobDate === null || endJobDate > firstDayOfMonth)) {
                            startMonthWorking++;
                        } 
                        if (startJobDate.getMonth() === index && startJobDate.getFullYear() === selectedYear) {
                            if (item.Gender === "Kadın") {
                                monthGetPersonelWoman++;
                            }
                            else if (item.Gender === "Erkek"){
                                monthGetPersonelMan++;
                            }
                            monthGetPersonel++;
                        }
                    }
                    if (item.EndJobDate) { // EĞER İŞTEN ÇIKIŞ TARİHİ VARSA
                        let endJobDate = new Date(item.EndJobDate);
                        if (endJobDate.getMonth() === index && endJobDate.getFullYear() === selectedYear) {
                            if (item.Gender === "Kadın") {
                                monthBanPersonelWoman++;
                            }
                            else if (item.Gender === "Erkek"){
                                monthBanPersonelMan++;
                            }
                            monthBanPersonel++;
                        }
                    }
                });

                // Satıra değerleri ekle
                row.innerHTML = `
                        <td>${month}</td>
                        <td>${startMonthWorking}</td>
                       <td>${monthGetPersonelMan > 0 ? `${monthGetPersonelMan} Erkek` : ""}  ${monthGetPersonelWoman > 0 ? `${monthGetPersonelWoman} Kadın` : ""} ${monthGetPersonelMan <= 0 && monthGetPersonelWoman <= 0 ? "Yok" : ""}</td>
                         <td>${monthBanPersonelMan > 0 ? `${monthBanPersonelMan} Erkek` : ""}  ${monthBanPersonelWoman > 0 ? `${monthBanPersonelWoman} Kadın` : ""} ${monthBanPersonelMan <= 0 && monthBanPersonelWoman <= 0 ? "Yok" : ""}</td>
                        <td>${startMonthWorking + monthGetPersonel - monthBanPersonel}</td> 
        `;

                // Tabloya satırı ekle
                tableBody.appendChild(row);
            }
        });
    }
    //Personel Sayısı Fonkisiyon
    function PersonalCountSec(PersonalCount) {
        $('#personalCount').text(PersonalCount);
    }
    //Maaş Verisi Hesaplama Fonksiyonu
    function SalarySum(SalaryCount) {
        const formattedSalary = SalaryCount.toLocaleString('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        });
        $('#salarySum').text(formattedSalary);
    }
    //Gıda Yardım Hesaplama Fonksiyonu
    function FoodAidSum(FoodAidCount) {
        const formattedSalary = FoodAidCount.toLocaleString('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        });
        $('#foodAidSum').text(formattedSalary);
    }
    //Cinsiyet Dağılımı Grafik Fornkisyonu
    function genderPie(menLength, womenLenght) {
        let options = {
            series: [menLength, womenLenght],
            title: {
                text: 'Personel Cinsiyet Dağılımı'
            },
            chart: {
                type: 'pie',
            },
            legend: {
                position: 'bottom'
            },
            dataLabels: {
                enabled: true,
                textAnchor: 'start',
                style: {
                    fontSize: '18px',
                    color: '#ffffff'
                },
            },
            colors: ['#0052cc', '#d63939'],
            labels: ['Erkek', 'Kadın'],
            responsive: [{
                breakpoint: 480,
                options: {
                    chart: {
                        width: 200
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }]

        }
        let chart = new ApexCharts(document.getElementById("genderGraph"), options);
        chart.render();
    }
    //Eğitim Durumu Grafik Fonksiyonu
    function educationPie(educationCounts) {
        let seriesData = [];
        let labels = [];
        for (const [key, value] of Object.entries(educationCounts)) {
            labels.push(key);
            seriesData.push(value.count);
        }
        let options = {
            series: seriesData,
            title: {
                text: 'Personel Eğitim Dağılımı'
            },
            chart: {
                type: 'donut',
            },
            legend: {
                position: 'bottom' // Taşıma işlemi burada gerçekleşiyor
            },
            dataLabels: {
                enabled: true,

                style: {
                    fontSize: '18px',
                    colors: ['#ffffff']
                },
            },
            labels: labels,
            responsive: [{
                breakpoint: 480,
                options: {
                    chart: {
                        width: 200
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            }]

        }
        let chart = new ApexCharts(document.getElementById("educationGraph"), options);
        chart.render();
    }
    //Doğum Tarihi Fonksiyonu
    function birthSection(birthDate,kalanGunSayisi,personalName) {
        let listItem = document.createElement('div');
        listItem.className = 'list-group-item';
        let row = document.createElement('div');
        row.className = 'row';
        let avatarCol = document.createElement('div');
        avatarCol.className = 'col-auto';
        avatarCol.innerHTML = `<span class="avatar">${personalName.charAt(0)}</span>`;
        let infoCol = document.createElement('div');
        infoCol.className = 'col';
        let remainingText = (birthDate.getDate() - today.getDate() === 0) ? "Bugün" :
            (birthDate.getDate() - today.getDate() === 1) ? "Yarın" :
                `${kalanGunSayisi} Gün Sonra`;
        infoCol.innerHTML = `
                <div class="text-truncate">
                    <strong>${personalName}</strong> doğum günü yaklaşıyor.
                </div>
                <div class="text-secondary">${birthDate.toLocaleDateString('tr-TR', {day: 'numeric', month: 'long'})} - ${remainingText}</div>
            `;
        

        row.appendChild(avatarCol);
        row.appendChild(infoCol);
        if(birthDate.getDate() - today.getDate() === 0){ // Eğer bugün Doğdu ise mavi tik koy
            let badgeCol = document.createElement('div');
            badgeCol.className = 'col-auto align-self-center';
            badgeCol.innerHTML = '<div class="badge bg-primary"></div>';
            row.appendChild(badgeCol);
            listItem.className += " active";
        }
        listItem.appendChild(row);
        birthList.appendChild(listItem);
    }
    //Personel kısımlerı get metodu
    $.ajax({
        type: "GET",
        url: "/query/personel-sayisi?expand=PersonalDetails($select=Salary,educationStatus),Branch($select=ID,Name)&$select=id,PersonalDetails,gender,birthDate,nameSurname,StartJobDate,EndJobDate,Status,foodAid",
    })
        .done(function (res) {
        $('#genderGraphSpinner').hide()
        $('#educationGraphSpinner').hide()
        $('#workTableSpinner').hide()
        $('#birthListSpinner').hide()
        personalResponse = res;
        // Tüm kişileri doğum tarihine göre sırala
        res.sort(function(a, b) {
            let dateA = new Date(2000, new Date(a.BirthDate).getMonth(), new Date(a.BirthDate).getDate());
            let dateB = new Date(2000, new Date(b.BirthDate).getMonth(), new Date(b.BirthDate).getDate());

            return dateA - dateB;
        });
        let birthListCount = 0;
        res.forEach(function (p) {

            if (p.Status === 0) { 
                PersonalCount++;
            }// Personel Sayısı Alanı
            if (p.PersonalDetails && p.PersonalDetails.Salary && p.Status === 0) { 
                SalaryCount += parseFloat(p.PersonalDetails.Salary);
            }// Maaş Alanı
            if (p.Gender === 'Erkek' && p.Status === 0) {
                erkekSayisi++;
            }else if (p.Gender === 'Kadın' && p.Status === 0) { // Cinsiyet Alanı
                kadinSayisi++;
            }
            //Gıda Yardım Alanı
            if (p.FoodAid && p.Status === 0){
                FoodAidCount += parseFloat(p.FoodAid)
            }
            if (p.BirthDate && p.Status === 0) {
                let birthDate = new Date(p.BirthDate);
                let dogumgununtarihi = new Date(today.getFullYear(), birthDate.getMonth(), birthDate.getDate());
                let kalanGunSayisi = Math.ceil((dogumgununtarihi - today) / (1000 * 60 * 60 * 24));
                if (kalanGunSayisi <= 10 && kalanGunSayisi >= 0) {
                    birthListCount++;
                    birthSection(birthDate,kalanGunSayisi,p.NameSurname);
                }
            } //Doğum gunu alanı 
            if (p.Status === 0) {
                //Eğitim Durumu Alanı Dinamik
                let educationStatus = p.PersonalDetails.EducationStatus ? p.PersonalDetails.EducationStatus : 'Yok';
                if (!educationCounts[educationStatus]) {
                    educationCounts[educationStatus] = { count: 0 };
                }
                educationCounts[educationStatus].count++;
            //Eğitim Durumu Alanı Dinamik
            }
        });
        if (birthListCount <= 0){
            $('#birthListEmptyText').removeClass('d-none')
        }
        PersonalCountSec(PersonalCount);
        SalarySum(SalaryCount);
        FoodAidSum(FoodAidCount);
        WorkPowerTable(res);
        genderPie(erkekSayisi, kadinSayisi)
        educationPie(educationCounts);
        
    });
    //Şube sayısı ajax metoddu
    $.ajax({
        type: "GET",
        url: "/query/sube-sayisi?$select=name,id&$orderby=name"
    }).done(function (res) {
        $('#branchCount').text(res.length === 0 ? "Yok" : res.length);

        $('[name="filterBranch"]').empty();
        $.each(res, function (index , branch) {
            $('[name="filterBranch"]').append(`<option value='${branch.ID}'>${branch.Name}</option>`);
        });
        let branchTomSelect = new TomSelect($('[name="filterBranch"]'),{
            plugins: {
                'clear_button':{
                    'title':'Tüm Şubeleri Temizle',
                },
                remove_button:{
                    title:'Şubeyi Temizle',
                }
            }, 
        });
        branchTomSelect.clear();
    });
    //Ünvan sayısı ajax metoddu
    $.ajax({
        type: "GET",
        url: "/query/unvan-sayisi"
    }).done(function (res) {
        $('#positionCount').text(res.length === 0 ? "Yok" : res.length);
    });
    //Bekleyen İzinler ajax metodu
    $.ajax({
        type:"GET",
        url: "/query/bekleyen-izinler-dashboard?$expand=Personal($select=ID,NameSurname;)&$select=id,createdAt,offdaystatus,Personal&$filter=Personal/Status eq 'Online'&$orderby=createdAt desc",
    }).done(function (res) {
        $('#waitingOffDaySpinner').hide();
        if (res.length > 0){
            res.forEach(data=> {
                let listItem = document.createElement('div');
                listItem.className = 'list-group-item';
                let row = document.createElement('div');
                row.className = 'row';
                let avatarCol = document.createElement('div');
                avatarCol.className = 'col-auto';
                avatarCol.innerHTML = `<span class="avatar">${data.Personal.NameSurname.charAt(0)}</span>`;
                let infoCol = document.createElement('div');
                infoCol.className = 'col';
                infoCol.innerHTML = `
                <div class="text-truncate">
                    <strong>${data.Personal.NameSurname}</strong> adlı personele ait bekleyen izin var.
                </div>
                <div class="text-secondary">Oluşturulma Tarihi - ${new Date(data.CreatedAt).toLocaleDateString('tr-TR', { day: 'numeric', month: 'long', year: 'numeric' })}</div>
            `;

                row.appendChild(avatarCol);
                row.appendChild(infoCol);
                let badgeCol = document.createElement('div');
                badgeCol.className = 'col-auto align-self-center';
                let badgeDiv = document.createElement('div');
                if(data.OffDayStatus === 1){
                    badgeDiv.className = 'badge bg-primary';
                    badgeDiv.textContent =  "GM Onayı Bekliyor";
                    badgeCol.appendChild(badgeDiv)
                    listItem.className += " active";
                }
                else{
                    badgeDiv.className = 'badge bg-yellow';
                    badgeDiv.textContent =  "IK Onayı Bekliyor";
                    badgeCol.appendChild(badgeDiv)
                    listItem.className += " active";
                    listItem.style.borderLeftColor = "rgb(245 159 0)"
                }
                row.appendChild(badgeCol);
                listItem.appendChild(row);
                offDayList.appendChild(listItem);
            })
        }
        else{
            $('#waitingOffDayEmptyText').removeClass('d-none'); 
        }
    })
    //Eksik Gün istirahat ajax metodu
    $.ajax({
        type:"GET",
        url: "/query/eksik-gun-dashboard?$expand=Personal($select=NameSurname,Status;)&$select=id,EndOffDayDate,Personal,EndOffDayDate,Reason&$filter=Personal/Status eq 'Online'and Reason eq 'İstirahat (01)'&$orderby=EndOffDayDate asc",
    }).done(function (res) {
        $('#missingDaySpinner').hide();
        if (res.length > 0) {
            res.forEach(data => {
                let raporTarihi = new Date(data.EndOffDayDate)
                let kalanGunSayisi = Math.ceil((raporTarihi - today) / (1000 * 60 * 60 * 24));
                if (kalanGunSayisi <= 5 && kalanGunSayisi >= 0) {
                    let remainingText = (raporTarihi.getDate() - today.getDate() === 0) ? "Bugün" :
                        (raporTarihi.getDate() - today.getDate() === 1) ? "Yarın" :
                            `${kalanGunSayisi} Gün Sonra`;
                    let listItem = document.createElement('div');
                    listItem.className = 'list-group-item';
                    let row = document.createElement('div');
                    row.className = 'row';
                    let avatarCol = document.createElement('div');
                    avatarCol.className = 'col-auto';
                    avatarCol.innerHTML = `<span class="avatar">${data.Personal.NameSurname.charAt(0)}</span>`;
                    let infoCol = document.createElement('div');
                    infoCol.className = 'col';
                    infoCol.innerHTML = `
                <div class="text-truncate">
                    <strong>${data.Personal.NameSurname}</strong> adlı personele ait rapor tarihi yaklaşıyor.
                </div>
                <div class="text-secondary">Rapor Bitiş Tarihi: ${new Date(data.EndOffDayDate).toLocaleDateString('tr-TR', { day: 'numeric', month: 'long' })} - ${remainingText}</div>
            `;

                    row.appendChild(avatarCol);
                    row.appendChild(infoCol);
                    if (raporTarihi.getDate() - today.getDate() === 0) {
                        let badgeCol = document.createElement('div');
                        badgeCol.className = 'col-auto align-self-center';
                        badgeCol.innerHTML = '<div class="badge bg-success"></div>';
                        row.appendChild(badgeCol);
                        listItem.className += " active";
                        listItem.style.borderLeftColor = "rgb(47, 179, 68)"
                    }
                    listItem.appendChild(row);
                    missDayList.appendChild(listItem);
                }

            })
        }
        else {
            $('#missingDayEmptyText').removeClass('d-none');
            
          
        }
    })
    //İş gücü veri tablosu filtrele butonu tıklandığında calısan metod
    $('#yearButton').on('click',function () {
        $('#tableBody').empty();
        $('.dropdown-menu').removeClass('show');
        let selectedYearInput = parseInt($("select[name='filterYear']").val(),10) || today.getFullYear();
        let selectedBranches = $('[name="filterBranch"]').val()
        WorkPowerTable(personalResponse,selectedYearInput,selectedBranches);
    });
});