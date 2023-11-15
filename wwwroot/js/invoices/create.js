let products = [];

function addProduct() {
  products.push({
    id: "",
    price: 0,
    quantity: 1,
    image: "https://via.placeholder.com/75x45",
    total: 0,
  });

  drawTable();
}

function newInvoice() {
  document.querySelector("form#form-invoice").reset();

  products = [
    {
      id: "",
      price: 0,
      quantity: 1,
      image: "https://via.placeholder.com/75x45",
      total: 0,
    },
  ];

  drawTable();
}

function createInvoice(event) {
  event.preventDefault();

  const form = event.target;
  const formData = new FormData(form);

  const prods = products.filter(
    (product) => product.id !== "" && product.quantity > 0
  );

  const subTotal = products.reduce(
    (total, product) => total + product.total,
    0
  );

  const taxTotal = subTotal * 0.19;

  const details = prods.map((product) => ({
    productId: product.id,
    productQuantity: product.quantity,
    productPrice: product.price,
    subTotal: product.total,
  }));

  const invoice = {
    customerId: formData.get("invoice-client"),
    number: formData.get("invoice-number"),
    totalNumberItems: prods.reduce(
      (total, product) => total + parseFloat(product.quantity),
      0
    ),
    details,
    subTotal,
    taxTotal,
    total: subTotal + taxTotal,
  };

  if (invoice.customerId === "" || invoice.number === "") {
    Swal.fire("", "Debes ingresar todos los datos de la factura", "warning");
    return;
  } else if (prods.length === 0) {
    Swal.fire("", "Debes ingresar al menos un producto", "warning");
    return;
  }

  Swal.fire({
    title: "¿Estás seguro?",
    text: "¿Estás seguro de crear la factura con los datos ingresados?",
    icon: "info",
    showCancelButton: true,
    confirmButtonText: "Sí, crear",
    cancelButtonText: "No, cancelar",
  }).then((result) => {
    if (result.value) {
      saveInvoice(invoice);
    }
  });
}

function drawTable() {
  const table = document.getElementById("products-table");
  const tableBody = table.querySelector("tbody");
  const row = tableBody.rows[tableBody.rows.length - 1];
  tableBody.innerHTML = "";

  products.forEach((product, index) => {
    newRow = row.cloneNode(true);

    const select = newRow.cells[0].querySelector("select");
    const input = newRow.cells[2].querySelector("input");

    select.value = product.id;
    select.setAttribute("data-item", index);
    newRow.cells[1].innerHTML = format(product.price);
    input.value = product.quantity;
    input.setAttribute("data-item", index);
    newRow.cells[3].innerHTML = drawImage(product.image);
    newRow.cells[4].innerHTML = format(product.total);

    tableBody.append(newRow);
  });

  const tableFoot = table.querySelector("tfoot");

  const subtotal = products.reduce(
    (total, product) => total + product.total,
    0
  );

  const taxes = subtotal * 0.19;

  tableFoot.rows[0].querySelector(".value-field").innerHTML = format(subtotal);
  tableFoot.rows[1].querySelector(".value-field").innerHTML = format(taxes);
  tableFoot.rows[2].querySelector(".value-field").innerHTML = format(
    subtotal + taxes
  );
}

async function getProduct(id) {
  try {
    const response = await fetch(`/Products/GetProductById/${id}`);
    return await response.json();
  } catch (error) {
    Swal.fire(
      "Error",
      "Ocurrió un problema al cargar los datos, intenta nuevamente",
      "error"
    );

    console.error("Error:", error);
  }
}

async function saveInvoice(invoice) {
  try {
    const response = await fetch(`/Invoices/Create`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(invoice),
    });

    if (response.status === 201) {
      Swal.fire("", "La factura se creó correctamente", "success");
      newInvoice();
    } else if (response.status === 409) {
      Swal.fire(
        "",
        "Ya existe una factura con el mismo número, intenta nuevamente",
        "warning"
      );
    }
  } catch (error) {
    Swal.fire(
      "Error",
      "Ocurrió un problema al intentar guardar la factura, intenta nuevamente",
      "error"
    );

    console.error("Error:", error);
  }
}

function drawImage(image) {
  return `<img src="${image}" alt="Imagen del producto" height="50" />`;
}

function format(number) {
  const formatter = new Intl.NumberFormat("es-CO");
  return formatter.format(parseFloat(number.toFixed(2)));
}

async function selectProduct(select) {
  const index = select.getAttribute("data-item");

  const product = await getProduct(select.value);

  if (!product) {
    return;
  }

  products[index].id = product.id;
  products[index].price = product.unitPrice;
  products[index].quantity = products[index].quantity;
  products[index].image = product.image;
  products[index].total = products[index].quantity * product.unitPrice;

  drawTable();
}

function updateQuantity(input) {
  const index = input.getAttribute("data-item");

  products[index].quantity = input.value;
  products[index].total = products[index].price * products[index].quantity;

  drawTable();
}

document.addEventListener("DOMContentLoaded", () => {
  addProduct();
});
