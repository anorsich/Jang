﻿var jang = {
    templatesDownloaded: false,
    templatesDownloading: false,
    pendingCallbacks: [],
    views: []
};

jang.__render = function (template, model)
{
    {viewEngineRender}
};

jang.renderView = function (view, model) {
	var viewName = jang.__findView(view);
	return jang.__render(viewName, model);
};

jang.__scrub = function (text) {
	var scrubber = /[^A-Za-z0-9_\\-\\s]/g;
	var spaceRemover = /\\s+/g;
	text = text.replace(scrubber, "-").trim();
	text = text.replace(spaceRemover, "-");
	text = text.replace( /^-*/ , "");
	text = text.replace( /-*$/ , "");
	text = text.replace( /--+/ , "-");
	text = text.toLowerCase();
	return text;
}

jang.__findView = function (view) {
	if (view[0] == '~') { //absolute path
		view = jang.__scrub(view);
		return view;
	}

	view = jang.__scrub(view);
	for (var i in jang.views) {
		if (jang.views[i].view == view) {
			return jang.views[i].fullPath;
		}
	}

	return null;
};

jang.render = function (id, destination, model) {
	//queue this up for rendering
	jang.__ensureTemplatesExist(function () {
		console.log(destination);
		var result = $(jang.__render(id, model));
		console.log(result);
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

jang.__ensureTemplatesExist = function (callback) {
	if (!jang.templatesDownloaded && !jang.templatesDownloading) {
		jang.templatesDownloading = true;

		$.ajax({
			url: '/jang/views',
			success: function (result) {
				$('body').append(result);
				jang.templatesDownloaded = true;
				jang.__ensureTemplatesExist(callback);
				for (var i in jang.pendingCallbacks) {
					console.log(i);
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