$(document).ready(function ()
{
    $(".active").removeClass("active");
    $(".homeNav").addClass("active");
    getBalance();
    getInventory();

    $("#1bill").on("click", function (e) {
        sendMoney(1);
    })

    $("#5bill").on("click", function (e) {
        sendMoney(5);
    })

    $("#10bill").on("click", function (e) {
        sendMoney(10);
    })

    $("#change").on("click", getChange);
});

let yourBalance = 0;

function getChange() {
    if (yourBalance > 0) {
        $.ajax({
            url: "http://localhost:55801/api/change",
            type: "POST"
        }).done(function (response) {
            let message = ""
            $("#feedBackStatus").text(response.Status.Status);
            message += "Your change: "

            if (response.Change.Dollars > 0) {
                message += `Dollars: ${response.Change.Dollars} `;
            }

            if (response.Change.Quarters > 0) {
                message += `Quarters: ${response.Change.Quarters} `;
            }

            if (response.Change.Dimes > 0) {
                message += `Dimes: ${response.Change.Dimes} `;
            }

            if (response.Change.Nickels > 0) {
                message += `Nickels: ${response.Change.Nickels} `;
            }

            if (response.Change.Pennies > 0) {
                message += `Pennies: ${response.Change.Pennies} `;
            }

            $("#feedBackImage").removeClass();
            $("#feedBackImage").addClass("fas far fa-coins fa-5x text-center card-img-top debug");
            $("#feedBackMessage").text(message);

            getBalance();
            getInventory();
        })
    }
    else {
        $("#feedBackStatus").text("Woops")
        $("#feedBackImage").removeClass();
        $("#feedBackImage").addClass("fas far fa-coins fa-5x text-center card-img-top debug");
        $("#feedBackMessage").text("You had no money")
    }
        
}

function sendMoney(amt) {
    $.ajax({
        url: "http://localhost:55801/api/feedmoney",
        type: "POST",
        data: {
            amount: amt
        }
    }).done(function (response) {
        getBalance();
        feedMessage(amt, response.Status);
        getInventory();
    })
}

function getInventory() {
    $.ajax({
        url: "http://localhost:55801/api/inventory",
        type: "GET",
        dataType: "json"
    }).done(function (data) {
        console.log("Inventory Loaded");
        for (let i = 0; i < data.length; i++) {
            card = $("<div>").addClass("card");
            body = $("<div>").addClass("card-body");
            title = $("<h5>").addClass("card-title");
            header = $("<h5>").addClass("card-header");
            image = $("<img>").addClass("card-img-top");
            price = $("<div>").addClass("card-footer");
            
            
            title.text(data[i].Product.Name);
            header.text(data[i].Product.Name);
            price.text(`$${data[i].Product.Price.toFixed(2)}`);
            image.attr("src", data[i].Product.Image);

            key = data[i].Inventory.Key;
            card.attr("id", key);

            col = data[i].Inventory.Column.toString();
            row = data[i].Inventory.Row.toString();
            image.addClass("itemImage");
            card.css({ "grid-column": col, "grid-row": row, "width": "18rem"});

            card.addClass("debug");
            card.data("Col", col);
            card.data("Row", row);
            card.append(header);
            card.append(image);
            
            //qty = $("<p>").addClass("card-text")
            //qty.text(`Qty: ${data[i].Inventory.Qty}`)
            //body.append(qty);

            card.append(body);
            card.append(price);
            if (data[i].Product.Price > yourBalance) {
                card.addClass("cantClick");
                card.on("click", notEnough);
            }
            else {
                card.on("click", purchaseProduct);
            }
            
            $("#itemContainer").append(card);
        }
    });
}

function notEnough(e) {
    updateStatus("fa-times-circle", "Not enough money", "Error");
}

function purchaseProduct() {
    let prodCol = $(this).data("Col");
    let prodRow = $(this).data("Row");

    $.ajax({
        url: "http://localhost:55801/api/purchase",
        type: "POST",
        dataType: "json",
        data: {
            row: prodRow,
            col: prodCol
        }
    }).done(function (data) {
        getBalance();
        let image = "fa-thumbs-up";
        if (data.Status === "Error") {
            image = "fa-times-circle";
        }
        else {
            data.Message = "";
        }
        updateStatus(image, data.Message, data.Status);
        getInventory();
    });
}

function feedMessage(amount, status) {
    updateStatus("fa-money-bill-wave", `$${amount} added to balance`, status);
}

function updateStatus(image, message, status) {
    $("#feedBackImage").removeClass();
    $("#feedBackImage").addClass(`fas far ${image} fa-5x text-center card-img-top debug`);
    $("#feedBackMessage").text(message);
    $("#feedBackStatus").text(status);
}



function getBalance() {
    $.ajax({
        url: "http://localhost:55801/api/balance",
        type: "GET",
        dataType: "json",
    }).done(function (data)
    {
        yourBalance = data;
        let balance = formatter.format(data)
        $("#balance").text(balance)
    });
}

const formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 2
});