document.addEventListener('DOMContentLoaded',function () {
    let BranchList = [];
    //Checkboxlar üzerinde dinleme metodu
    document.querySelectorAll('[name="role"]').forEach(function(radio) {
        radio.addEventListener('change', function() {
            if (this.value === 'director') {
                createBranchSelectElement(true); // multiple için true
                
            } else if(this.value === 'branchManager') {
                createBranchSelectElement(false); // multiple için false
            }
            else{
                // Genel Müdür dışındaki seçenekler seçildiğinde şube seçimi alanını kaldır
                let divSection = document.querySelector('#branchSection >.row > .col.mb-3');
                if (divSection) {
                    divSection.remove();
                }
            }
        });
    });
    //Şubeleri Çektiğimiz Metod
    $.ajax({
        type: "GET",
        url: "/query/sube-sayisi?$select=name,id&$orderby=name",
    }).done(function (res) {
        if (res.length > 0){
            BranchList = res;
        }
        else{
            $('#error-modal-message').text("Şubeler Getirilemedi")
            $('#error-modal').modal('show')
        }
    });
    //Şube İnputu Oluşturma Metod
    function createBranchSelectElement(multiple) {
        let divSectionClear = document.querySelector('#branchSection >.row > .col.mb-3');
        if (divSectionClear) {
            divSectionClear.remove();
        }
        let branchSelectDiv = document.createElement('div');
        branchSelectDiv.classList.add('row');
        branchSelectDiv.innerHTML = `
        <div class="col mb-3">
            <label for="branchSelect" class="form-label required">Şube Seçiniz</label>
            <select name="branchName" ${multiple ? 'multiple' : ''} id="branchSelect" class="form-select" placeholder="Şube Seçiniz!">
            </select>
        </div>
    `;
        let divSection = document.getElementById('branchSection');
        divSection.appendChild(branchSelectDiv);
        BranchList.forEach(function(branch) {
            let option = document.createElement('option');
            option.value = branch.ID;
            option.text = branch.Name;
            $('#branchSelect').append(option);
        });
        new TomSelect($('#branchSelect'));
    };
    //Formu Resetleme
    $('#addModal').on('hidden.bs.modal', function (e) {
        // Form alanınızı resetleme
        $('#addUserForm')[0].reset();
        let divSection = document.querySelector('#branchSection >.row > .col.mb-3');
        if (divSection) {
            divSection.remove();
        }
        $('#hrInput').prop('checked', true);
        $('#branchManagerInput').prop('checked', false);
        $('#directorInput').prop('checked', false);
    });
    //Kullanıcı Ekle Butonu basıldığında
    $('#addUserButton').click(function () {
        let formData = $("#addUserForm").serializeArray();
        console.log(formData)
    })
});