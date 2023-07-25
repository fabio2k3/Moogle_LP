#!/bin/bash

run() {
  echo "Ejecutando el proyecto..."
  dotnet run --moogleFabioAlonsoC-111
}

clean() {
  echo "Eliminando ficheros auxiliares..."
  rm -rf
}

report() {
  echo "Compilando y generando el informe..."
  pdflatex Informe Moogle Fabio-C111.tex
}

slides() {
  echo "Compilando y generando la presentación..."
  pdflatex presentacion moogle.tex
}

show_report() {
  if [ ! -f "informe.pdf" ]; then
    report
  fi

  echo "Mostrando el informe..."
  cat Informe Moogle Fabio-C111.tex
}

show_slides() {
  if [ ! -f "presentacion.pdf" ]; then
    slides
  fi

  echo "Mostrando la presentación..."
  cat presentacion moogle.tex
}

# Verificar el número de argumentos proporcionados
if [ $# -lt 1 ]; then
  echo "Uso: proyecto.sh {run|clean|report|slides|show_report|show_slides}"
  exit 1
fi

# Ejecutar la opción seleccionada
case $1 in
  run)
    run
    ;;
  clean)
    clean
    ;;
  report)
    report
    ;;
  slides)
    slides
    ;;
  show_report)
    show_report
    ;;
  show_slides)
    show_slides
    ;;
  *)
    echo "Opción inválida"
    exit 1
    ;;
esac