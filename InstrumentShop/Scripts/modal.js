document.addEventListener('DOMContentLoaded', function () {
    console.log('DOMContentLoaded event triggered');

    document.getElementById('showEditModalBtn').addEventListener('click', function () {
        console.log('showEditModalBtn clicked');
        document.getElementById('editModal').style.display = 'block';
    });

    document.getElementById('showDetailModalBtn').addEventListener('click', function () {
        console.log('showDetailModalBtn clicked');
        document.getElementById('detailModal').style.display = 'block';
    });

    document.getElementById('showSupDetailModalBtn').addEventListener('click', function () {
        console.log('showSupDetailModalBtn clicked');
        document.getElementById('supDetailModal').style.display = 'block';
    });

    document.getElementById('showEditSupModalBtn').addEventListener('click', function () {
        console.log('showEditSupModalBtn clicked');
        document.getElementById('editSupModal').style.display = 'block';
    });

    document.getElementById('showAddProductBtn').addEventListener('click', function () {
        console.log('showAddProductBtn clicked');
        document.getElementById('addProd').style.display = 'block';
    });

    document.getElementById('showEditProdModalBtn').addEventListener('click', function () {
        console.log('showEditProdModalBtn clicked');
        document.getElementById('editProdModal').style.display = 'block';
    });

    document.getElementById('showAddSuppBtn').addEventListener('click', function () {
        console.log('showAddSuppBtn clicked');
        document.getElementById('addSupp').style.display = 'block';
    });

    document.getElementById('showProdDetailModalBtn').addEventListener('click', function () {
        console.log('showProdDetailModalBtn clicked');
        document.getElementById('prodDetailModal').style.display = 'block';
    });

    document.getElementById('closeBtn').addEventListener('click', function () {
        console.log('closeBtn clicked');
        document.getElementById('editModal').style.display = 'none';
        document.getElementById('detailModal').style.display = 'none';
        document.getElementById('supDetailModal').style.display = 'none';
        document.getElementById('editSupModal').style.display = 'none';
        document.getElementById('addProd').style.display = 'none';
        document.getElementById('editProdModal').style.display = 'none';
        document.getElementById('addSupp').style.display = 'none';
        document.getElementById('prodDetailModal').style.display = 'none';
    });

    document.getElementById('mainCloseBtn').addEventListener('click', function () {
        console.log('mainCloseBtn clicked');
        document.getElementById('editModal').style.display = 'none';
        document.getElementById('detailModal').style.display = 'none';
        document.getElementById('supDetailModal').style.display = 'none';
        document.getElementById('editSupModal').style.display = 'none';
        document.getElementById('addProd').style.display = 'none';
        document.getElementById('editProdModal').style.display = 'none';
        document.getElementById('addSupp').style.display = 'none';
        document.getElementById('prodDetailModal').style.display = 'none';
    });

    window.addEventListener('click', function (event) {
        console.log('window clicked');
        if (event.target == document.getElementById('editModal')) {
            document.getElementById('editModal').style.display = 'none';
        }
        if (event.target == document.getElementById('detailModal')) {
            document.getElementById('detailModal').style.display = 'none';
        }
        if (event.target == document.getElementById('supDetailModal')) {
            document.getElementById('supDetailModal').style.display = 'none';
        }
        if (event.target == document.getElementById('editSupModal')) {
            document.getElementById('editSupModal').style.display = 'none';
        }
        if (event.target == document.getElementById('addProd')) {
            document.getElementById('addProd').style.display = 'none';
        }
        if (event.target == document.getElementById('editProdModal')) {
            document.getElementById('editProdModal').style.display = 'none';
        }
        if (event.target == document.getElementById('addSupp')) {
            document.getElementById('addSupp').style.display = 'none';
        }
        if (event.target == document.getElementById('prodDetailModal')) {
            document.getElementById('prodDetailModal').style.display = 'none';
        }
    });
});


function openEditModal(userId, fname, mi, lname, department, phone, address, email, uname, pword, depList) {
    console.log('openEditModal called');
    document.getElementById('editModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('editModal')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Edit Staff</h1>
            </div>
            <div class="form-container">
            <div class="form-group">
                <label for="userId">ID:</label>
                <input type="text" id="userId" name="userId" class="textbox-style" style="width: 300px !important;" value="${userId}" required autofocus  oninput="updateModalContent('userId', this.value)" />
            </div>
            <div class="form-group">
                <label for="fname">Firstname:</label>
                <input type="text" id="fname" name="fname" class="textbox-style" style="width: 300px !important;" value="${fname}" required autofocus  oninput="updateModalContent('fname', this.value)" />
            </div>
            <div class="form-group">
                <label for="mi">M.I:</label>
                <input type="text" id="mi" name="mi" class="textbox-style" style="width: 300px !important;" value="${mi}" required  oninput="updateModalContent('mi', this.value)" />
            </div>
            <div class="form-group">
                <label for="lname">Lastname:</label>
                <input type="text" id="lname" name="lname" class="textbox-style" style="width: 300px !important;" value="${lname}" required  oninput="updateModalContent('lname', this.value)" />
            </div>
           
            <div class="form-group">
                <label for="phone">Phone:</label>
                <input type="text" id="phone" name="phone" class="textbox-style" style="width: 300px !important;" value="${phone}" required oninput="updateModalContent('phone', this.value)" />
            </div>
            <div class="form-group">
                <label for="address">Address:</label>
                <input type="text" id="address" name="address" class="textbox-style" style="width: 300px !important;" value="${address}" required oninput="updateModalContent('address', this.value)" />
            </div>
            <div class="form-group">
                <label for="email">Email:</label>
                <input type="text" id="email" name="email" class="textbox-style" style="width: 300px !important;" value="${email}" required oninput="updateModalContent('email', this.value)" />
            </div>
             <div class="form-group">
                <label for="uname">User:</label>
                <input type="text" id="uname" name="uname" class="textbox-style" style="width: 300px !important;" value="${uname}" required oninput="updateModalContent('uname', this.value)" />
            </div>
            <div class="form-group">
                <label for="pword">Password:</label>
                <input type="text" id="pword" name="pword" class="textbox-style" style="width: 300px !important;" value="${pword}" requiredoninput="updateModalContent('pword', this.value)" />
            </div>  
        </div>
        <div class="form-group">
        <label for="Department">Deparment:</label>
        <select id="Department" name="Department" class="textbox-style" style="width: 300px !important;" required autofocus oninput="updateModalContent('department', this.value)">
            ${depList.map(dep => `<option value="${dep.Value}" ${dep.Value === department ? 'selected' : ''}>${dep.Text}</option>`).join('')}
        </select>
        </div>
            <div class="form-group">
                <button type="button" onclick="editStaff('${userId}')">Submit</button>
            </div>

            </br></br>
        </div>
    `;

    document.getElementById('editModal').style.display = 'block';
}

function editStaff(userId) {
    console.log('editStaff called');
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", '/Staff/EditStaff');

    function createHiddenInput(name, value) {
        var input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", name);
        input.setAttribute("value", value);
        form.appendChild(input);
    }

    var updatedUserId = document.getElementById('userId').value;
    var updatedFname = document.getElementById('fname').value;
    var updatedMi = document.getElementById('mi').value;
    var updatedLname = document.getElementById('lname').value;
    var updatedDepartment = document.getElementById('Department').value;
    var updatedPhone = document.getElementById('phone').value;
    var updatedAddress = document.getElementById('address').value;
    var updatedEmail = document.getElementById('email').value;
    var updatedUname = document.getElementById('uname').value;
    var updatedPword = document.getElementById('pword').value;

    createHiddenInput("userId", updatedUserId);
    createHiddenInput("fname", updatedFname);
    createHiddenInput("mi", updatedMi);
    createHiddenInput("lname", updatedLname);
    createHiddenInput("department", updatedDepartment);
    createHiddenInput("phone", updatedPhone);
    createHiddenInput("address", updatedAddress);
    createHiddenInput("email", updatedEmail);
    createHiddenInput("uname", updatedPword);
    createHiddenInput("pword", updatedUname);

    document.body.appendChild(form);
    form.submit();
}



function openSupEditModal(suppid,company, fname, mi, lname, phone, address, email) {
    document.getElementById('editSupModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('editSupModal')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Edit Supplier</h1>
            </div>
            <div class="form-container">
            <div class="form-group">
                <label for="suppid">ID:</label>
                <input type="text" id="suppid" name="suppid" class="textbox-style" style="width: 300px !important;" value="${suppid}" required autofocus  oninput="updateModalContent('suppid', this.value)" />
            </div>
            <div class="form-group">
                <label for="company">Company:</label>
                <input type="text" id="company" name="company" class="textbox-style" style="width: 300px !important;" value="${company}" required autofocus  oninput="updateModalContent('company', this.value)" />
            </div>
            <div class="form-group">
                <label for="fname">Firstname:</label>
                <input type="text" id="fname" name="fname" class="textbox-style" style="width: 300px !important;" value="${fname}" required autofocus  oninput="updateModalContent('fname', this.value)" />
            </div>
            <div class="form-group">
                <label for="mi">M.I:</label>
                <input type="text" id="mi" name="mi" class="textbox-style" style="width: 300px !important;" value="${mi}" required  oninput="updateModalContent('mi', this.value)" />
            </div>
            <div class="form-group">
                <label for="lname">Lastname:</label>
                <input type="text" id="lname" name="lname" class="textbox-style" style="width: 300px !important;" value="${lname}" required  oninput="updateModalContent('lname', this.value)" />
            </div>
            <div class="form-group">
                <label for="phone">Phone:</label>
                <input type="text" id="phone" name="phone" class="textbox-style" style="width: 300px !important;" value="${phone}" required oninput="updateModalContent('phone', this.value)" />
            </div>
            <div class="form-group">
                <label for="address">Address:</label>
                <input type="text" id="address" name="address" class="textbox-style" style="width: 300px !important;" value="${address}" required oninput="updateModalContent('address', this.value)" />
            </div>
            <div class="form-group">
                <label for="email">Email:</label>
                <input type="text" id="email" name="email" class="textbox-style" style="width: 300px !important;" value="${email}" required oninput="updateModalContent('email', this.value)" />
            </div>               
        </div>
            <div class="form-group">
                <button type="button" onclick="editSupplier('${suppid}')">Submit</button>
            </div>

            </br></br>
        </div>
    `;


    document.getElementById('editSupModal').style.display = 'block';
}

function editSupplier(suppid) {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", '/Supplier/EditSupplier');

    function createHiddenInput(name, value) {
        var input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", name);
        input.setAttribute("value", value);
        form.appendChild(input);
    }

    var updatedSuppId = document.getElementById('suppid').value;
    var updatedCompany = document.getElementById('company').value;
    var updatedFname = document.getElementById('fname').value;
    var updatedMi = document.getElementById('mi').value;
    var updatedLname = document.getElementById('lname').value;
    var updatedPhone = document.getElementById('phone').value;
    var updatedAddress = document.getElementById('address').value;
    var updatedEmail = document.getElementById('email').value;

    createHiddenInput("suppid", updatedSuppId);
    createHiddenInput("company", updatedCompany);
    createHiddenInput("fname", updatedFname);
    createHiddenInput("mi", updatedMi);
    createHiddenInput("lname", updatedLname);
    createHiddenInput("phone", updatedPhone);
    createHiddenInput("address", updatedAddress);
    createHiddenInput("email", updatedEmail); 

    document.body.appendChild(form);
    form.submit();
}

function openProdEditModal(prodId, prodCode, prodCat, prodName, prodDesc, prodPrice, qoh, supplier, supList,compName) {
    console.log('prodEditModal called');
    document.getElementById('editProdModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('editProdModal')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Edit Product</h1>
            </div>
            <div class="form-container">
            
            <div class="form-group">
                <label for="prodId">ID:</label>
                <input type="text" id="prodId" name="prodId" class="textbox-style" style="width: 300px !important;" value="${prodId}" required autofocus  oninput="updateModalContent('prodId', this.value)" />
            </div>
            <div class="form-group">
                <label for="prodCode">Code:</label>
                <input type="text" id="prodCode" name="prodCode" class="textbox-style" style="width: 300px !important;" value="${prodCode}" required autofocus  oninput="updateModalContent('prodCode', this.value)" />
            </div>
            <div class="form-group">
                <label for="prodCat">Category:</label>
                <input type="text" id="prodCat" name="prodCat" class="textbox-style" style="width: 300px !important;" value="${prodCat}" required  oninput="updateModalContent('prodCat', this.value)" />
            </div>
            <div class="form-group">
                <label for="prodName">Name:</label>
                <input type="text" id="prodName" name="prodName" class="textbox-style" style="width: 300px !important;" value="${prodName}" required  oninput="updateModalContent('prodName', this.value)" />
            </div>
            <div class="form-group">
                <label for="prodDesc">Description:</label>
                <input type="text" id="prodDesc" name="prodDesc" class="textbox-style" style="width: 300px !important;" value="${prodDesc}" required oninput="updateModalContent('prodDesc', this.value)" />
            </div>
            <div class="form-group">
                <label for="prodPrice">Price:</label>
                <input type="text" id="prodPrice" name="prodPrice" class="textbox-style" style="width: 300px !important;" value="${prodPrice}" required oninput="updateModalContent('prodPrice', this.value)" />
            </div>
            <div class="form-group">
                <label for="qoh">Quantity on Hand:</label>
                <input type="number" min="0" id="qoh" name="qoh" class="textbox-style" style="width: 300px !important;" value="${qoh}" required oninput="updateModalContent('qoh', this.value)" />
            </div>
            <div class="form-group">
                <label for="compName">Supplier:</label>
                <input type="text" id="compName" name="compName" class="textbox-style" style="width: 300px !important;" value="${compName}" />
            </div>
           
             <div class="form-group">
                <label for="SupplierSelect">Update Supplier:</label>
                <select id="SupplierSelect" name="SupplierSelect" class="textbox-style" style="width: 300px !important;" required autofocus oninput="updateModalContent('supplier', this.value)">
                    ${supList.map(comp => `<option value="${comp.Value}" ${comp.Value === supplier ? 'selected' : ''}>${comp.Text}</option>`).join('')}
                </select>
            </div>
        </div>
            <div class="form-group">
                <button type="button" onclick="editProduct('${prodId}')">Submit</button>
            </div>

            </br></br>
        </div>
    `;

    document.getElementById('editProdModal').style.display = 'block';
}

function editProduct(prodId) {
    console.log('supId called');
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", '/Inventory/updateProduct');

    function createHiddenInput(name, value) {
        var input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", name);
        input.setAttribute("value", value);
        form.appendChild(input);
    }

    var updatedProdId = document.getElementById('prodId').value;
    var updatedProdName = document.getElementById('prodName').value;
    var updatedProdCat = document.getElementById('prodCat').value;
    var updatedProdDesc = document.getElementById('prodDesc').value;
    var updatedProdPrice = document.getElementById('prodPrice').value;
    var updatedQoh = document.getElementById('qoh').value;
    var updatedSup = document.getElementById('SupplierSelect').value;

    createHiddenInput("prodId", updatedProdId);
    createHiddenInput("prodName", updatedProdName);
    createHiddenInput("prodCat", updatedProdCat);
    createHiddenInput("prodDesc", updatedProdDesc);
    createHiddenInput("prodPrice", updatedProdPrice);
    createHiddenInput("qoh", updatedQoh);
    createHiddenInput("supId", updatedSup);


    document.body.appendChild(form);
    form.submit();
}

function openAddSupplier(company, fname, mi, lname, address, email, phone) {
    company = company || "";
    fname = fname || "";
    mi = mi || "";
    lname = lname || "";
    address = address || "";
    email = email || "";
    phone = phone || "";
    document.getElementById('addSupp').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('addSupp')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Add Supplier</h1>
            </div>
            <div class="form-container">
             <div class="form-group">
                <label for="company">Company:</label>
                <input type="text" id="company" name="company" class="textbox-style" style="width: 300px !important;" value="${company}" required autofocus  oninput="updateModalContent('company', this.value)" />
            </div>
            <div class="form-group">
                <label for="fname">Firstname:</label>
                <input type="text" id="fname" name="fname" class="textbox-style" style="width: 300px !important;" value="${fname}" required autofocus  oninput="updateModalContent('fname', this.value)" />
            </div>
            <div class="form-group">
                <label for="mi">M.I:</label>
                <input type="text" id="mi" name="mi" class="textbox-style" style="width: 300px !important;" value="${mi}" required autofocus  oninput="updateModalContent('mi', this.value)" />
            </div>
            <div class="form-group">
                <label for="lname">Lastname:</label>
                <input type="text" id="lname" name="lname" class="textbox-style" style="width: 300px !important;" value="${lname}" required autofocus  oninput="updateModalContent('lname', this.value)" />
            </div>
            <div class="form-group">
                <label for="address">Address:</label>
                <input type="text" id="address" name="address" class="textbox-style" style="width: 300px !important;" value="${address}" required autofocus  oninput="updateModalContent('address', this.value)" />
            </div>
             <div class="form-group">
                <label for="email">Email:</label>
                <input type="text" id="email" name="email" class="textbox-style" style="width: 300px !important;" value="${email}" required autofocus  oninput="updateModalContent('email', this.value)" />
            </div>
             <div class="form-group">
                <label for="phone">Phone:</label>
                <input type="text" id="phone" name="phone" class="textbox-style" style="width: 300px !important;" value="${phone}" required autofocus  oninput="updateModalContent('phone', this.value)" />
            </div>
            <div class="form-group">
                <button type="button" onclick="addSupplier()">Submit</button>
            </div>
            </br></br>
        </div>
    `;

    document.getElementById('addSupp').style.display = 'block';
}


function addSupplier() {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", '/Supplier/AddSupplier');

    function createHiddenInput(name, value) {
        var input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", name);
        input.setAttribute("value", value);
        form.appendChild(input);
    }

    var sComp = document.getElementById('company').value;
    var sFname = document.getElementById('fname').value;
    var sMi = document.getElementById('mi').value;
    var sLname = document.getElementById('lname').value;
    var sAddress = document.getElementById('address').value;
    var sEmail = document.getElementById('email').value;
    var sPhone = document.getElementById('phone').value;

    createHiddenInput("company", sComp);
    createHiddenInput("fname", sFname);
    createHiddenInput("mi", sMi);
    createHiddenInput("lname", sLname);
    createHiddenInput("address", sAddress);
    createHiddenInput("email", sEmail);
    createHiddenInput("phone", sPhone);

    document.body.appendChild(form);
    form.submit();
}


function openAddProduct(prodName, prodCat, prodDesc, prodPrice, qoh) {
    prodName = prodName || "";
    prodCat = prodCat || "";
    prodDesc = prodDesc || "";
    prodPrice = prodPrice || "";
    qoh = qoh || "";
    document.getElementById('addProd').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('addProd')" style="font-size: 35px !important;"></i>
            <div class="centered-text">
                <h1>Add Product</h1>
            </div>
            <div class="form-container">
             <div class="form-group">
                <label for="prodName">Product Name:</label>
                <input type="text" id="prodName" name="prodName" class="textbox-style" style="width: 300px !important;" value="${prodName}" required autofocus  oninput="updateModalContent('prodName', this.value)" />
            </div>
            <div class="form-group">
                <label for="prodCat">Product Category:</label>
                <input type="text" id="prodCat" name="prodCat" class="textbox-style" style="width: 300px !important;" value="${prodCat}" required autofocus  oninput="updateModalContent('prodCat', this.value)" />
            </div>
            <div class="form-group">
                <label for="prodDesc">Product Description:</label>
                <input type="text" id="prodDesc" name="prodDesc" class="textbox-style" style="width: 300px !important;" value="${prodDesc}" required autofocus  oninput="updateModalContent('prodDesc', this.value)" />
            </div>
            <div class="form-group">
                <label for="prodPrice">Product Price:</label>
                <input type="text" id="prodPrice" name="prodPrice" class="textbox-style" style="width: 300px !important;" value="${prodPrice}" required autofocus  oninput="updateModalContent('prodPrice', this.value)" />
            </div>
            <div class="form-group">
                <label for="qoh">Quantity on Hand:</label>
                <input type="number" min="0" id="qoh" name="qoh" class="textbox-style" style="width: 300px !important;" value="${qoh}" required autofocus  oninput="updateModalContent('qoh', this.value)" />
            </div>
            <div class="form-group">
                <button type="button" onclick="addProduct()">Submit</button>
            </div>
            </br></br>
        </div>
    `;

    document.getElementById('addProd').style.display = 'block';
}


function addProduct() {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", '/Inventory/AddProduct');

    function createHiddenInput(name, value) {
        var input = document.createElement("input");
        input.setAttribute("type", "hidden");
        input.setAttribute("name", name);
        input.setAttribute("value", value);
        form.appendChild(input);
    }

    var pName = document.getElementById('prodName').value;
    var pCat = document.getElementById('prodCat').value;
    var pDesc = document.getElementById('prodDesc').value;
    var pPrice = document.getElementById('prodPrice').value;
    var qoh = document.getElementById('qoh').value;

    createHiddenInput("prodName", pName);
    createHiddenInput("prodCat", pCat);
    createHiddenInput("prodDesc", pDesc);
    createHiddenInput("prodPrice", pPrice);
    createHiddenInput("qoh", qoh);

    document.body.appendChild(form);
    form.submit();
}

function updateModalContent(fieldId, fieldValue) {
    document.getElementById(fieldId).value = fieldValue;
}
function openDetailModal(userId, fname, mi, lname, department, status,phone,email,address) {
    document.getElementById('detailModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('detailModal')" style="font-size: 35px !important;"></i>
            
            <div class="centered-text">
                <h1>Staff Details</h1>
            </div>
            <div class="details-container">
                <p><strong>Firstname:</strong> ${fname}</p>
                <p><strong>M.I:</strong> ${mi}</p>
                <p><strong>Lastname:</strong> ${lname}</p>
                <p><strong>Department:</strong>${department}</p>
                <p><strong>Status:</strong>${status}</p>
                <p><strong>Phone:</strong>${phone}</p
                <p><strong>Email:</strong>${email}</p>
                <p><strong>Address:</strong>${address}</p>
                <!-- Add more fields as needed -->
            </div>

            <!-- Add any additional content for viewing details -->

        </div>
    `;
    document.getElementById('detailModal').style.display = 'block';
}

function openSupDetailModal(suppid,company, fname, mi, lname, phone, email,address) {
    document.getElementById('supDetailModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('supDetailModal')" style="font-size: 35px !important;"></i>
            
            <div class="centered-text">
                <h1>Supplier Details</h1>
            </div>
            <div class="details-container">
                <p><strong>Company:</strong> ${company}</p>
                <p><strong>Firstname:</strong> ${fname}</p>
                <p><strong>M.I:</strong> ${mi}</p>
                <p><strong>Lastname:</strong> ${lname}</p>
                <p><strong>Phone:</strong>${phone}</p
                <p><strong>Email:</strong>${email}</p>
                <p><strong>Address:</strong>${address}</p>
                <!-- Add more fields as needed -->
            </div>

            <!-- Add any additional content for viewing details -->

        </div>
    `;
    document.getElementById('supDetailModal').style.display = 'block';
}

function openProdDetailModal(prodCode, prodName, prodDesc, prodPrice, qoh, company) {
    console.log('gitawag si product detail');
    document.getElementById('prodDetailModal').innerHTML = `
        <div class="modal-content">
            <i class='bx bx-arrow-back' onclick="closeModal('prodDetailModal')" style="font-size: 35px !important;"></i>
            
            <div class="centered-text">
                <h1>Product Details</h1>
            </div>
            <div class="details-container">
                <p><strong>Product Code:</strong> ${prodCode}</p>
                <p><strong>Product Name:</strong> ${prodName}</p>
                <p><strong>Product Description:</strong> ${prodDesc}</p>
                <p><strong>Product Price:</strong> ${prodPrice}</p>
                <p><strong>Quantity on Hand:</strong>${qoh}</p
                <p><strong>Supplier:</strong>${company}</p>
                <!-- Add more fields as needed -->
            </div>

            <!-- Add any additional content for viewing details -->

        </div>
    `;
    document.getElementById('prodDetailModal').style.display = 'block';
    console.log('gi tawag siya');
}

function closeModal(modalId) {
    document.getElementById(modalId).style.display = 'none';
}