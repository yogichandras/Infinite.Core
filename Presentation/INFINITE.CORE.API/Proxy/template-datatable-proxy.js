path_name: function (targetElementId, columns) {
    var url = path_url;
    return sendDataTableRequest(method, url, targetElementId, null, columns);
}