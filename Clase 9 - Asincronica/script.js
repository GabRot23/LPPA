const nombreCompleto = document.getElementById("nombreCompleto");
const email = document.getElementById("email");
const password = document.getElementById("password");
const repetirPassword = document.getElementById("repetirPassword");
const telefono = document.getElementById("telefono");
const edad = document.getElementById("edad");
const direccion = document.getElementById("direccion");
const ciudad = document.getElementById("ciudad");
const codPostal = document.getElementById("codPostal");
const dni = document.getElementById("dni");
var forms = document.querySelectorAll(".needs-validation");
// Bucle sobre ellos y evitar el envío
Array.prototype.slice.call(forms).forEach(function (form) {
  form.addEventListener(
    "submit",
    function (event) {
      if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
      }

      form.classList.add("was-validated");
    },
    false
  );
});

// Validación de Nombre Completo

function validarNombre() {
  const nombreValido = nombreCompleto.value;
  const regNombre = new RegExp(/^(?=.*[A-Za-z]+\s[A-z]+).{6,}$/);

  if (regNombre.test(nombreValido)) {
    nombreCompleto.classList.add("is-valid");
  } else {
    nombreCompleto.classList.add("is-invalid");
  }
}

function ocultarErrorNombre() {
  nombreCompleto.classList.remove("is-invalid");
  nombreCompleto.classList.remove("is-valid");
}

nombreCompleto.addEventListener("blur", validarNombre);
nombreCompleto.addEventListener("focus", ocultarErrorNombre);

// Validación de Email

function validarEmail() {
  const emailValido = email.value;
  const regEmail = /\w+@\w+\.[a-z]/;
  if (regEmail.test(emailValido)) {
    email.classList.add("is-valid");
  } else {
    email.classList.add("is-invalid");
  }
}

function ocultarErrorEmail() {
  email.classList.remove("is-invalid");
  email.classList.remove("is-valid");
}

email.addEventListener("blur", validarEmail);
email.addEventListener("focus", ocultarErrorEmail);

// Validación de Password

function validarPassword() {
  const passwordValido = password.value;
  const expresion = new RegExp("^(?=.*?[a-z])(?=.*?[0-9]).{8,}$");
  if (expresion.test(passwordValido)) {
    password.classList.add("is-valid");
  } else {
    password.classList.add("is-invalid");
  }
}

function ocultarErrorPassword() {
  password.classList.remove("is-invalid");
  password.classList.remove("is-valid");
}

password.addEventListener("blur", validarPassword);
password.addEventListener("focus", ocultarErrorPassword);

// Validación de Repetir Password

function validarRepetirPassword() {
  const passwordValido = password.value;
  const repetirPasswordValido = repetirPassword.value;
  console.log(passwordValido, repetirPasswordValido);
  if (passwordValido === repetirPasswordValido) {
    repetirPassword.classList.add("is-valid");
  } else {
    repetirPassword.classList.add("is-invalid");
  }
}

function ocultarErrorRepetirPassword() {
  repetirPassword.classList.remove("is-invalid");
  repetirPassword.classList.remove("is-valid");
}

repetirPassword.addEventListener("blur", validarRepetirPassword);
repetirPassword.addEventListener("focus", ocultarErrorRepetirPassword);

// Validación de Telefono

function validarTelefono() {
  const telefonoValido = telefono.value;
  if (telefonoValido.length > 6) {
    telefono.classList.add("is-valid");
  } else {
    telefono.classList.add("is-invalid");
  }
}

function ocultarErrorTelefono() {
  telefono.classList.remove("is-invalid");
  telefono.classList.remove("is-valid");
}

telefono.addEventListener("blur", validarTelefono);
telefono.addEventListener("focus", ocultarErrorTelefono);

// Validación de Edad

function validarEdad() {
  const edadValido = edad.value;
  const regEdad = new RegExp("^[0-9]{2}$");
  if (regEdad.test(edadValido) && edadValido >= 18) {
    edad.classList.add("is-valid");
  } else {
    edad.classList.add("is-invalid");
  }
}

function ocultarErrorEdad() {
  edad.classList.remove("is-invalid");
  edad.classList.remove("is-valid");
}

edad.addEventListener("blur", validarEdad);
edad.addEventListener("focus", ocultarErrorEdad);

// Validación de Direccion

function validarDireccion() {
  const direccionValido = direccion.value;
  const regDireccion = new RegExp(/[A-Za-z]+\s[0-9]+/i);
  if (regDireccion.test(direccionValido) && direccionValido.length > 4) {
    direccion.classList.add("is-valid");
  } else {
    direccion.classList.add("is-invalid");
  }
}

function ocultarErrorDireccion() {
  direccion.classList.remove("is-invalid");
  direccion.classList.remove("is-valid");
}

direccion.addEventListener("blur", validarDireccion);
direccion.addEventListener("focus", ocultarErrorDireccion);

// Validación de Ciudad

function validarCiudad() {
  const ciudadValido = ciudad.value;
  if (ciudadValido.length > 2) {
    ciudad.classList.add("is-valid");
  } else {
    ciudad.classList.add("is-invalid");
  }
}

function ocultarErrorCiudad() {
  ciudad.classList.remove("is-invalid");
  ciudad.classList.remove("is-valid");
}

ciudad.addEventListener("blur", validarCiudad);
ciudad.addEventListener("focus", ocultarErrorCiudad);

// Validación de Codigo Postal

function validarCodPostal() {
  const codPostalValido = codPostal.value;
  if (codPostalValido.length > 2) {
    codPostal.classList.add("is-valid");
  } else {
    codPostal.classList.add("is-invalid");
  }
}

function ocultarErrorCodPostal() {
  codPostal.classList.remove("is-invalid");
  codPostal.classList.remove("is-valid");
}

codPostal.addEventListener("blur", validarCodPostal);
codPostal.addEventListener("focus", ocultarErrorCodPostal);

// Validacion de DNI

function validarDni() {
  const dniValido = dni.value;
  if (dniValido.length > 6 && dniValido.length < 9) {
    dni.classList.add("is-valid");
  } else {
    dni.classList.add("is-invalid");
  }
}

function ocultarErrorDni() {
  dni.classList.remove("is-invalid");
  dni.classList.remove("is-valid");
}

dni.addEventListener("blur", validarDni);
dni.addEventListener("focus", ocultarErrorDni);
