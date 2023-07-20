var service = {
    notification: {

        get: function (targetElementId, data, errorHandler) {
            var url = '/notification/get';
            return sendRequest('GET', url, targetElementId, data, false, errorHandler);
        },

        count: function (targetElementId, data, errorHandler) {
            var url = '/notification/count';
            return sendRequest('GET', url, targetElementId, data, false, errorHandler);
        },

        list: function (targetElementId, columns) {
            var url = '/notification/list';
            return sendDataTableRequest('POST', url, targetElementId, null, columns);
        },

        add: function (targetElementId, data, errorHandler) {
            var url = '/notification/add';
            return sendRequest('POST', url, targetElementId, data, false, errorHandler);
        },

        open: function (targetElementId, data, errorHandler) {
            var url = '/notification/open';
            return sendRequest('PUT', url, targetElementId, data, false, errorHandler);
        },

        delete: function (targetElementId, data, errorHandler) {
            var url = '/notification/delete';
            return sendRequest('DELETE', url, targetElementId, data, false, errorHandler);
        },

        push_notif: function (targetElementId, data, errorHandler) {
            var url = '/notification/push_notif';
            return sendRequest('POST', url, targetElementId, data, false, errorHandler);
        }
    },
    repository: {

        upload: function (targetElementId, data, errorHandler) {
            var url = '/repository/upload';
            return sendRequest('POST', url, targetElementId, data, false, errorHandler);
        },

        delete: function (targetElementId, data, errorHandler) {
            var url = '/repository/delete';
            return sendRequest('DELETE', url, targetElementId, data, false, errorHandler);
        },

        download: function (targetElementId, data, errorHandler) {
            var url = '/repository/download';
            return sendRequest('GET', url, targetElementId, data, false, errorHandler);
        },

        list: function (targetElementId, columns) {
            var url = '/repository/list';
            return sendDataTableRequest('POST', url, targetElementId, null, columns);
        }
    },
    role: {

        get: function (targetElementId, data, errorHandler) {
            var url = '/role/get';
            return sendRequest('GET', url, targetElementId, data, false, errorHandler);
        },

        list: function (targetElementId, columns) {
            var url = '/role/list';
            return sendDataTableRequest('POST', url, targetElementId, null, columns);
        },

        add: function (targetElementId, data, errorHandler) {
            var url = '/role/add';
            return sendRequest('POST', url, targetElementId, data, false, errorHandler);
        },

        edit: function (targetElementId, data, errorHandler) {
            var url = '/role/edit';
            return sendRequest('PUT', url, targetElementId, data, false, errorHandler);
        },

        active: function (targetElementId, data, errorHandler) {
            var url = '/role/active';
            return sendRequest('PUT', url, targetElementId, data, false, errorHandler);
        },

        delete: function (targetElementId, data, errorHandler) {
            var url = '/role/delete';
            return sendRequest('DELETE', url, targetElementId, data, false, errorHandler);
        },

        list_permissions: function (targetElementId, data, errorHandler) {
            var url = '/role/list-permissions';
            return sendRequest('GET', url, targetElementId, data, false, errorHandler);
        }
    },
    user: {

        register: function (targetElementId, data, errorHandler) {
            var url = '/user/register';
            return sendRequest('POST', url, targetElementId, data, false, errorHandler);
        },

        login: function (targetElementId, data, errorHandler) {
            var url = '/user/login';
            return sendRequest('POST', url, targetElementId, data, false, errorHandler);
        },

        get: function (targetElementId, data, errorHandler) {
            var url = '/user/get';
            return sendRequest('GET', url, targetElementId, data, false, errorHandler);
        },

        list: function (targetElementId, columns) {
            var url = '/user/list';
            return sendDataTableRequest('POST', url, targetElementId, null, columns);
        },

        edit_info: function (targetElementId, data, errorHandler) {
            var url = '/user/edit_info';
            return sendRequest('POST', url, targetElementId, data, false, errorHandler);
        },

        logoff: function (targetElementId, data, errorHandler) {
            var url = '/user/logoff';
            return sendRequest('POST', url, targetElementId, data, false, errorHandler);
        },

        lock: function (targetElementId, data, errorHandler) {
            var url = '/user/lock';
            return sendRequest('PUT', url, targetElementId, data, false, errorHandler);
        },

        active: function (targetElementId, data, errorHandler) {
            var url = '/user/active';
            return sendRequest('PUT', url, targetElementId, data, false, errorHandler);
        },

        change_password: function (targetElementId, data, errorHandler) {
            var url = '/user/change_password';
            return sendRequest('PUT', url, targetElementId, data, false, errorHandler);
        },

        reset_password: function (targetElementId, data, errorHandler) {
            var url = '/user/reset_password';
            return sendRequest('PUT', url, targetElementId, data, false, errorHandler);
        },

        refresh_token: function (targetElementId, data, errorHandler) {
            var url = '/user/refresh_token';
            return sendRequest('POST', url, targetElementId, data, false, errorHandler);
        }
    },
    userrole: {

        get: function (targetElementId, data, errorHandler) {
            var url = '/userrole/get';
            return sendRequest('GET', url, targetElementId, data, false, errorHandler);
        },

        list: function (targetElementId, columns) {
            var url = '/userrole/list';
            return sendDataTableRequest('POST', url, targetElementId, null, columns);
        },

        add: function (targetElementId, data, errorHandler) {
            var url = '/userrole/add';
            return sendRequest('POST', url, targetElementId, data, false, errorHandler);
        },

        edit: function (targetElementId, data, errorHandler) {
            var url = '/userrole/edit';
            return sendRequest('PUT', url, targetElementId, data, false, errorHandler);
        },

        delete: function (targetElementId, data, errorHandler) {
            var url = '/userrole/delete';
            return sendRequest('DELETE', url, targetElementId, data, false, errorHandler);
        }
    }
}