// Service Account
var service = {
    user: {
        login: function (targetElementId, data, errorHandler) {
            var url = "/User/login";
            return sendRequest("POST", url, targetElementId, data, false, errorHandler);
        },
    },
    role: {
        get: function (targetElementId, data, errorHandler) {
            var url = '/Role/get';
            return sendRequest("GET", url, targetElementId, data, false, errorHandler)
        },
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
        },
        edit: function (targetElementId, data, errorHandler) {
            var url = '/Role/edit';
            return sendRequest("PUT", url, targetElementId, data, false, errorHandler);
        },
        delete: function (targetElementId, data, errorHandler) {
            var url = '/Role/delete';
            return sendRequest("DELETE", url, targetElementId, data, false, errorHandler);
        },
    }
    // Tambahkan service lain di sini
};