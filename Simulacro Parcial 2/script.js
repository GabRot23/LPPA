// Version: 1.0
// Autor: Gabriel Rotela
// Fecha: 2019/05/16
// Descripcion: Script para el simulacro del parcial 2

//Variables del DOM
let inputsDim2 = document.getElementById("inputsDim2");
let inputsDim3 = document.getElementById("inputsDim3");
let ax2 = document.getElementById("2ax");
let ay2 = document.getElementById("2ay");
let bx2 = document.getElementById("2bx");
let by2 = document.getElementById("2by");
let ax3 = document.getElementById("3ax");
let ay3 = document.getElementById("3ay");
let az3 = document.getElementById("3az");
let bx3 = document.getElementById("3bx");
let by3 = document.getElementById("3by");
let bz3 = document.getElementById("3bz");

let form = document.getElementById("formCalculo");
let dimensionalidad = document.getElementById("dimensionalidad");

document.addEventListener(
    "DOMContentLoaded",
    function () {
        if (dimensionalidad.value == 2) {
            inputsDim2.style.display = "block";
            inputsDim3.style.display = "none";
        } else if (dimensionalidad.value == 3) {
            inputsDim3.style.display = "block";
            inputsDim2.style.display = "none";
        }
    },
    false
);

//add event listener for change in a select
dimensionalidad.addEventListener(
    "change",
    function () {
        if (dimensionalidad.value == 2) {
            inputsDim2.style.display = "block";
            inputsDim3.style.display = "none";
        } else if (dimensionalidad.value == 3) {
            inputsDim3.style.display = "block";
            inputsDim2.style.display = "none";
        }
    },
    false
);

form.addEventListener(
    "submit",
    function (event) {
        event.preventDefault();
        event.stopPropagation();
        let distancia;
        console.log(dimensionalidad.value);

        if (dimensionalidad.value == 2) {
            distancia = calcularDistancia2D(
                ax2.value,
                ay2.value,
                bx2.value,
                by2.value
            );
        } else if (dimensionalidad.value == 3) {
            distancia = calcularDistancia3D(
                ax3.value,
                ay3.value,
                az3.value,
                bx3.value,
                by3.value,
                bz3.value
            );
        } else {
            alert("Error en la dimensionalidad");
            return;
        }
        alert("La distancia es: " + distancia);
    },
    false
);

//Algoritmo para calcular distancia entre dos puntos en dos dimensiones
function calcularDistancia2D(x1, y1, x2, y2) {
    const numeros = [x1, y1, x2, y2];
    console.log(numeros);
    try {
        numeros.forEach((element) => {
            console.log(typeof element);
            validarInts(element);
        });
    } catch (error) {
        alert(error.message);
        return;
    }

    let distancia = Math.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2));
    return distancia;
}

//Algoritmo para calcular distancia entre dos puntos en tres dimensiones
function calcularDistancia3D(x1, y1, z1, x2, y2, z2) {
    let distancia = Math.sqrt(
        Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2) + Math.pow(z2 - z1, 2)
    );
    return distancia;
}

const validarInts = (value) => {
    if (isNaN(value) || value == "") {
        throw new Error("Se ingresó un valor no numérico");
    }
};
