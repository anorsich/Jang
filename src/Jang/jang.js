var jang = {
    templatesDownloaded: false,
    templatesDownloading: false,
    pendingCallbacks: []
};

jang.__render = function (template, model)
{
    {viewEngineRender}
};

jang.render = function (id, destination, model) {
    //queue this up for rendering
    jang.ensureTemplatesExist(function () {
        $('#' + destination).after($(jang.__render(id, model)));
    });
};

jang.ajax = function (settings) {
    var ajaxSettings = settings || {};
    var finalSuccess = settings.success || function (e) {
        throw "not really sure if this is an error or not";
    };

    ajaxSettings.success = function (result) {
        finalSuccess(jang.__render(result.template, result.model));
    };

    $.ajax(ajaxSettings);
};

jang.ensureTemplatesExist = function (callback) {
    if (!jang.templatesDownloaded && !jang.templatesDownloading) {
        jang.templatesDownloading = true;

        $.ajax({
            url: '/jang/views',
            success: function (result) {
                $('body').append(result);
                jang.templatesDownloaded = true;
                jang.ensureTemplatesExist(callback);
                for (var i in jang.pendingCallbacks) {
                    jang.pendingCallbacks[i]();
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    } else if (jang.templatesDownloading) {
        jang.pendingCallbacks.push(callback);
    } else {
        callback();
    }
};