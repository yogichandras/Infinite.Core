// Service Account
var service = {
    user: {
        login: function (targetElementId, data, errorHandler) {
            var url = "/User/login"; // Ganti dengan path URL endpoint yang sesuai
            return sendRequest("POST", url, targetElementId, data, false, errorHandler);
        },
        // Tambahkan method lain di sini
    },
    role: {
        list_permissions: function (targetElementId, data, errorHandler) {
            var url = '/Role/list-permissions';
            return sendRequest("GET", url, targetElementId, data, false, errorHandler);
        },
        list: function (targetElementId, columns) {
            var url = '/Role/list';
            return sendDataTableRequest("POST", url, targetElementId, 'name', columns);
        },
        add: function (targetElementId, data, errorHandler) {
            var url = '/Role/add';
            return sendRequest("POST", url, targetElementId, data, false, errorHandler);
        }
    }
    // Tambahkan service lain di sini
};