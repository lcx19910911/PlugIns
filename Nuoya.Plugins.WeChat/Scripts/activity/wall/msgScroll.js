var msg = (
    function ()
{
        var a = 176;
        var c = 1000;
        var b = 3000;
        var d = function (e)
        {
            this.length = 0;
            this.current = 0;
            this.wrapper = null;
            this.container = null;
            this.timer = true;
            this.auto = e.auto; 
            this.onanimate = false;
            this.data = []
        };
        d.prototype =
            {
                init: function (f)
                {
                    var e = this;
                    e.container = $(f);
                    e.wrapper = e.container.parent();
                    e.length = 0;
                    return e
                },
                prev: function ()
                {
                    var e = this;
                    if (e.onanimate)
                    {
                        return
                    }
                    e.current++;
                    e.onanimate = true;
                    e.stopAuto().startAuto();
                    e.go();
                    return e
                },
                next: function ()
                {
                    var e = this;
                    if (e.onanimate)
                    {
                        return
                    }
                    e.current--;
                    e.onanimate = true;
                    e.stopAuto().startAuto();
                    e.go();
                    return e
                },
                stopAuto: function ()
                {
                    var e = this;
                    if (e.auto)
                    {
                        clearTimeout(e.timer)
                    }
                    return e
                },
                startAuto: function ()
                {
                    var e = this;
                    setTimeout(function ()
                    {
                        e.onanimate = false;
                        if (e.auto)
                        {
                            e.tick()
                        }
                    }, c);
                    return e
                },
                work: function ()
                {
                    var e = this;
                    if (e.auto)
                    {
                        e.tick()
                    }
                    else
                    {
                        e.go()
                    }
                    return e
                },
                tick: function ()
                {
                    var e = this;
                    e.timer = setTimeout(function ()
                    {
                        e.current++; e.go(); e.work()
                    }, b);
                    return e
                },
                go: function ()
                {
                    var e = this;
                    if (e.length <= 3)
                    {
                        return
                    }
                    if (e.current > e.length)
                    {
                        e.current = e.current - e.length;
                        e.container.css({ marginTop: 0 })
                    }
                    else
                    {
                        if ((e.current + e.length) < 0)
                        {
                            e.current = e.current + e.length; e.container.css({ marginTop: 0 })
                        }
                        else
                        { }
                    }
                    e.container.animate({ marginTop: -(e.current * a) }, c); return e
                },
                getMessage: function ()
                { }
            }; return d
})();