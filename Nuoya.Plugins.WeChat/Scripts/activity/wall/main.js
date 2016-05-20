function fullScreen(e) {
    e.preventDefault(),
	e.stopPropagation();
    var i = document.documentElement,
	r = i.webkitRequestFullscreen || i.requestFullScreen || i.webkitRequestFullScreen || i.mozRequestFullScreen || i.msRequestFullscreen;
    r && r.call(i);
};

function playMsc(thi, evt) {
    var msc = document.getElementById('msc');
    if (thi.className.indexOf("on") > -1) {
        msc.pause();
    } else {
        msc.play();
    }
    $(thi).toggleClass("on");
}