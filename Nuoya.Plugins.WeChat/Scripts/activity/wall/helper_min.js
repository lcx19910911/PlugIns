/*绗笁鏂规彃浠讹細html妯℃澘鐢熸垚鍣�*/
var iTemplate =
    (function ()
    {
        var a = function () { };
        a.prototype =
        {
            makeList: function (e, j, i)
            {
                var g = [], h = [], c = /{(.+?)}/g, d = {}, f = 0;
                for (var b in j)
                {
                    if (typeof i === "function")
                    {
                        d = i.call(this, b, j[b], f++) || {}
                    }
                    g.push(e.replace(c, function (k, l)
                    {
                        return (l in d) ? d[l] : (undefined === j[b][l] ? j[b] : j[b][l])
                    }))
                }
                return g.join("")
            }
        }; return new a()
    })();