var menuLeft, menuRight, cartList;
var totalNum = 0, totalPrice = 0;

function getCart() {
    var cart = null;
    try {
        cart = $.cookie('cart');
        if (!cart) cart = new Object();
        else cart = JSON.parse(cart);
    } catch (e) {
        cart = new Object();
    }

    return cart;
}

function leftMenuClick() {
    $(".left-menu ul li").removeClass("active");
    $(this).addClass("active");
    var cid = $(this).attr("data-id");
    $.ajax({
        url: "GetDish",
        type: "post",
        data: { dc_id: cid },
        cache: true,    //启用缓存
        async: false,
        success: function (rv) {
            if (rv.Code==0) {
                $(".right_bc").html("");
                $(rv.Result).each(function (index, o) {
                    $(".right_bc").append(initDish(o, cid));
                });
            }
        }
    });


    $("#right-menu .add-oper").each(function (i, e) {
        e.addEventListener('tap', addoper);
    });

    $("#right-menu .delete-oper").each(function (i, e) {
        e.addEventListener('tap', deloper);
    });

    refreshRightMenu(cid);
    menuRight.refresh();
}

function rightMenuClick() {
    var id = $(this).attr("data");
    window.location.href = "Details?name=" + escape($("#name" + id).text())
        + "&price=" + escape($("#price" + id).text())
        + "&img=" + escape($(this).attr("src"))
        + "&des=" + escape($(this).attr("d-data"))
        + "&tid=" + escape(id + "_" + $("#name" + id).attr("data"));
}

function deloper(id, cid) {
    if (!(id && cid)) {
        var data = $(this).attr("data").split("_");
        id = data[0];
        cid = data[1];
    }

    var cart = getCart();

    var tid = id + "_" + cid;
    if (!cart[tid]) cart[tid] = { "num": 0, "price": 0, "name": "" };
    else cart[tid]["num"] = cart[tid]["num"] - 1;


    refreshLeftMenu("remove", cid);
    refreshRightMenu(cid, id);
    refreshCart("remove", tid, cart);

    if (cart[tid]["num"] == 0) {
        delete cart[tid];
    }

    $.cookie('cart', JSON.stringify(cart), "/");
}

function addoper(id, cid,name) {
    if (!(id && cid)) {
        var data = $(this).attr("data").split("_");
        id = data[0];
        cid = data[1];
        name = data[2];
    }

    var cart = getCart();
    var tid = id + "_" + cid;
    if (!cart[tid]) cart[tid] = { "num": 1, "price": parseFloat($("#price" + id).attr("data-value")), name: $("#name" + id).text() };
    else cart[tid]["num"] = cart[tid]["num"] + 1;

    cart[tid]["name"] = name;

    var dish_h = $("#dish_h" + id);

    if (dish_h.hasClass("no-num")) {
        dish_h.removeClass("no-num");
    }
    dish_h.find(".num").text(cart[tid]["num"]);
    refreshLeftMenu("add", cid);
    refreshCart("add", tid, cart);
    $.cookie('cart', JSON.stringify(cart), "/");
}

function refreshLeftMenu(type, hid) {
    var o = $("#dish" + hid + " .number-tip");

    //删除
    if (type == "remove") {
        o.text(parseInt(o.text()) - 1);
        if (o.text() == "0") {
            o.text("0").hide();
        }
        return;
    }
    //新增
    if (type == "add") {
        o.show().text(parseInt(o.text()) + 1);
    }
    //刷新
    if (type == "refresh") {
        var cart = getCart();
        var hasgo = [];
        for (var p in cart) {
            var ids = p.split("_");
            var d = $("#dish" + ids[1] + " .number-tip");
            if (hasgo.indexOf(ids[1]) == -1) {
                hasgo.push(ids[1]);
                d.show().text(cart[p]["num"]);
            } else {
                d.text(parseInt(d.text()) + cart[p]["num"]);
            }
        }
    }
}

//不传id就是全部刷新
function refreshRightMenu(cid, id) {
    var cart = getCart();
    for (var p in cart) {
        var ids = p.split("_");
        if (ids[1] == cid) {
            if (id == ids[0]) {
                var o = $("#dish_h" + ids[0] + " .num");
                if (o.text() == "1") {
                    o.text("0");
                    $("#dish_h" + ids[0]).addClass("no-num");
                } else {
                    o.text(parseInt(o.text()) - 1);
                }
            } else {
                $("#dish_h" + ids[0]).removeClass("no-num");
                $("#dish_h" + ids[0] + " .num").text(cart[p]["num"]);
            }
        }
    }
}

/*更新购物车一栏的展示信息*/
function refreshCart(type, id, c) {
    var cart = null;
    if (!c) cart = getCart();
    else cart = c;

    if (Object.getOwnPropertyNames(cart).length == 0) {
        $("#cart").removeClass("not-empty").addClass("empty");
        $("#cartOK").removeAttr("disabled");
        return;
    }

    if (type == "refresh") {
        for (var p in cart) {
            totalNum += cart[p]["num"];
            totalPrice += cart[p]["num"] * cart[p]["price"];
        }
    }

    if (type == "add") {
        totalNum += 1;
        totalPrice += cart[id]["price"];
    }

    if (type == "remove") {
        totalNum -= 1;
        totalPrice -= cart[id]["price"];
    }

    if (totalNum > 0 && totalPrice > 0) {
        $("#cart").removeClass("empty").addClass("not-empty");
        $("#cartOK").removeAttr("disabled");

        $("#cartPrice .value").text(totalPrice);
        $("#cartNum").text(totalNum);
    } else {
        $("#cart").removeClass("not-empty").addClass("empty");
        $("#cartOK").attr("disabled", "disabled");
    }


}

$(function () {
    menuLeft = new IScroll('#left-menu', { mouseWheel: true, tap: true });
    menuRight = new IScroll('#right-menu', { mouseWheel: true, tap: true });
    cartList = new IScroll("#list", { mouseWheel: true, tap: true });
    $(".left-menu ul li").each(function (i, e) {
        e.addEventListener('tap', leftMenuClick);
    });

    document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);

    leftMenuClick.call($(".left-menu ul li:first")[0]);
    refreshLeftMenu("refresh");
    refreshCart("refresh");

    $(".overlay").click(function () {
        $(this).hide();
        $(".cart-detail").hide();
    });

    $("#cartOK").click(function (e) {

        e.stopPropagation();
        if (!$("#cart").hasClass("empty"))
            window.location.href = "OrderList";
    });

    $("#orderShow").click(function (e) {
        e.stopPropagation();
        window.location.href = "Haves";
    })
});

/*清除购物车清单*/
function clearAll(noConfirm) {

    var fn = function () {
        $.cookie('cart', "");
        $(".number-tip").text("0").hide();

        $("#right-menu .oper-container").addClass("no-num").find(".num").text(0);

        refreshCart();

        $(".cart-detail,.overlay").hide();
        totalNum = totalPrice = 0;
        $("#cart").removeClass("not-empty").addClass("empty");
        $("#cartOK").attr("disabled", "disabled");
    }

    if (noConfirm) {
        fn();
        return;
    }

    $("#confirm .am-modal-bd").text("确定清空点菜单吗？")
    $('#confirm').modal({
        onConfirm: fn
    })
}

/*显示购物车清单*/
function showCartList() {
    if (!$("#cart").hasClass("empty")) {
        $(".cart-detail,.overlay").show();
        initCartList();
    }
}

/*菜品列表构造函数*/
function initDish(o, c) {
    return '<li data-id="' + o.UNID + '" class="item clearfix">\
                    <div class="item-img-container">\
                        <img data=' + o.UNID + ' src="' +o.Image + '" data=' + o.Description + '>\
                    </div>\
                    <div class="item-label-container"></div>\
                    <div class="item-info">\
                        <p class="item-name">\
                            <span id="name' + o.UNID + '" data="' + c + '">' + o.Name + '</span>\
                        </p>\
                        <span class="J-price-container">\
                            <span id="price' + o.UNID + '" data-value="' + o.Price + '" class="price">¥' + o.Price.toFixed(2) + '</span>\
                            <span class="unit">/份</span>\
                        </span>\
                        <span id="dish_h' + o.UNID + '" class="oper-container no-num">\
                            <span data="' + o.UNID + '_' + c + '_'+o.Name+'" class="delete-oper oper-icon"></span>\
                            <span class="num">1</span>\
                            <span data="' + o.UNID + '_' + c + '_' + o.Name + '" class="add-oper oper-icon"></span>\
                        </span>\
                    </div>\
                </li>';
}

/*购物车清单构造函数*/
function initCartList() {
    var list = getCart();

    var listObj = $("#cartmenulist");
    listObj.html("");

    for (var d in list) {
        var da = d.split("_");

        var o = listObj.append(' <div class="item clearfix">\
                                    <div class="oper-container">\
                                        <span data="' + da[0] + '_' + da[1] + '" class="delete-oper oper-icon"></span>\
                                        <span class="num">' + list[d].num + '</span>\
                                        <span data="' + da[0] + '_' + da[1] + '" class="add-oper oper-icon"></span>\
                                    </div>\
                                    <div class="item-info">\
                                        <div class="item-name">' + list[d].name + '</div>\
                                        <div class="item-price" data="' + list[d].price + '">\
                                            <span>¥' +  (list[d].price*list[d].num) + '</span>\
                                        </div>\
                                    </div>\
                                </div>');
    }

    cartList.refresh();

    $("#right-menu .add-oper").each(function (i, e) {
        e.addEventListener('tap', listAddoper);
    });

    $("#cartmenulist .delete-oper").each(function (i, e) {
        e.addEventListener('tap', listDeloper);
    });

}

function listDeloper() {
    var n = $(this).next();
    var num = parseInt(n.text()) - 1;
    n.text(num);
    var p = n.parent().next().find(".item-price");
    p.children().text(parseFloat(p.attr("data")) * num);

    var data = $(this).attr("data").split("_");
    var id = data[0];
    var cid = data[1];

    deloper(id, cid);

    if (num == "0") {
        $(this).parent().parent().remove();
        if ($("#cartmenulist").children().length == 0) {
            clearAll(true);
        }
    }
}

function listAddoper() {
    var n = $(this).prev();
    var num = parseInt(n.text()) + 1;
    n.text(num);

    var data = $(this).attr("data").split("_");
    var id = data[0];
    var cid = data[1];
    var name = data[2];

    var p = n.parent().next().find(".item-price");
    p.children().text(parseFloat(p.attr("data")) * num);

    addoper(id, cid, name);
}
