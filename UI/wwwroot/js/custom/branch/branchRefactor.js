document.addEventListener('DOMContentLoaded',function () {
    let totalPages;
    const rowsPerPage = 10;
    let currentPage = 1;
    let searchKeyword = '';
    let url;
    fetchData();
   
    function fetchData(pageNumber = 1) {
        $.ajax({
            type: "GET",
            url: `/getbranch-list?pageNumber=${pageNumber}`,
        }).done(function (res) {
            console.log(res);
            renderTable(res.data); // OData sonucunun "value" özelliğini kullanın
                //totalPages = res.totalPages;
            
            totalPages = Math.ceil(res.totalRecords / rowsPerPage);
            updatePagination();
        });
    }
    function renderTable(data) {
        let tableBody = $("#table-body");

        // Tabloyu temizle
        tableBody.empty();

        // Her bir veri için bir satır oluşturun
        data.forEach(function (item) {
            let row = $("<tr>");
            row.append("<td>" + item.name + "</td>");
            row.append(`<td>${item.status === 0 ? '<span class="badge bg-success">Aktif</span>' : '<span class="badge bg-secondary">Pasif</span>'}</td>`);
            row.append(`<td>${new Date(item.createdAt).toLocaleDateString('tr-TR')}</td>`);
            row.append(`<td>${new Date(item.modifiedAt).toLocaleDateString('tr-TR')}</td>`);
            row.append(`<td><a href="#" class="btn btn-warning btn-sm text-white btn-pill w-50">Düzenle</a></td>`);
            row.append(`
    <td>
        <button type="button" class="btn btn-danger btn-sm btn-pill w-50 archive-button" 
            data-bs-toggle="modal" 
            data-bs-target="#archiveModal" 
            data-item-id="${item.id}" 
            data-item-personal="${item.name}">
            Sil
        </button>
    </td>
`);

            // Satırı tabloya ekle
            tableBody.append(row);
        });
    }
    function updatePagination() {
        const paginationContainer = $("#customPagination");
        paginationContainer.empty();

        const maxVisiblePages = 5;
        const startPage = Math.max(1, currentPage - Math.floor(maxVisiblePages / 2));
        const endPage = Math.min(totalPages, startPage + maxVisiblePages - 1);
        const firstPageItem = $("<li>", { class: currentPage === 1 ? "page-item disabled" : "page-item first" });
        const firstPageLink = $("<a>", { class: "w-33 h-px-30 page-link", href: "#" }).html(`
        <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-arrow-bar-to-left" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
            <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
            <path d="M10 12l10 0"></path>
            <path d="M10 12l4 4"></path>
            <path d="M10 12l4 -4"></path>
            <path d="M4 4l0 16"></path>
        </svg>
    `);
        firstPageLink.click(() => changePage(1));
        firstPageItem.append(firstPageLink);
        paginationContainer.append(firstPageItem);
        for (let i = startPage; i <= endPage; i++) {
            const listItem = $("<li>", { class: i === currentPage ? "page-item active" : "page-item" });
            const link = $("<a>", { class: "page-link", href: "#" }).text(i);
            link.click(() => changePage(i));
            listItem.append(link);
            paginationContainer.append(listItem);
        }
        const lastPageItem = $("<li>", { class: currentPage === totalPages ? "page-item disabled" : "page-item last" });
        const lastPageLink = $("<a>", { class: "w-33 h-px-30 page-link", href: "#" }).html(`
        <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-arrow-bar-to-right" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
            <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
            <path d="M14 12l-10 0"></path>
            <path d="M14 12l-4 4"></path>
            <path d="M14 12l-4 -4"></path>
            <path d="M20 4l0 16"></path>
        </svg>
    `);
        lastPageLink.click(() => changePage(totalPages));
        lastPageItem.append(lastPageLink);
        paginationContainer.append(lastPageItem);
    }
    function changePage(pageNumber) {
        currentPage = pageNumber;
        fetchData(pageNumber);
    }
});