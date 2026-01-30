# -*- coding: utf-8 -*-
"""
Created on Fri Jan 30 16:59:06 2026

@author: Admin
"""

import pandas as pd
import os
import subprocess  # Necesario para ejecutar otros procesos
import sys         # Necesario para saber dónde está el ejecutable de python

# Nombre del archivo de entrada
archivo_excel = 'VARMAP.xlsx'
siguiente_script = 'VARMAPGenerator.py'

def convertir_excel_a_csv():
    # Verificamos que el archivo exista antes de intentar abrirlo
    if not os.path.exists(archivo_excel):
        print(f"❌ Error: No se encontró el archivo '{archivo_excel}' en el directorio actual.")
        return

    print(f"📂 Leyendo '{archivo_excel}'...")

    try:
        # sheet_name=None es el truco para cargar TODAS las hojas en un diccionario
        # Las claves serán los nombres de las hojas y los valores los datos
        hojas = pd.read_excel(archivo_excel, sheet_name=None)

        for nombre_hoja, dataframe in hojas.items():
            nombre_csv = f"{nombre_hoja}.csv"
            
            # Guardamos en CSV
            # index=False: Evita que se guarde la columna de numeración de filas (0,1,2...)
            # encoding='utf-8-sig': Asegura que tildes y ñ se vean bien en Excel
            dataframe.to_csv(nombre_csv, index=False, encoding='utf-8-sig')
            
            print(f"✅ Hoja '{nombre_hoja}' convertida a -> {nombre_csv}")

        print("\n🎉 ¡Proceso terminado exitosamente!")

    except Exception as e:
        print(f"❌ Ocurrió un error inesperado: {e}")
        
def ejecutar_siguiente_paso():
    print(f"\n🚀 Iniciando ejecución de '{siguiente_script}'...")

    if not os.path.exists(siguiente_script):
        print(f"❌ Error: No encuentro el archivo '{siguiente_script}' para ejecutarlo.")
        return

    try:
        # sys.executable asegura que usamos el mismo Python que está corriendo este script
        # check=True hará que salte un error si el segundo script falla
        subprocess.run([sys.executable, siguiente_script], check=True)
        
        print(f"✅ '{siguiente_script}' finalizó correctamente.")

    except subprocess.CalledProcessError:
        print(f"⚠️ El script '{siguiente_script}' falló (tuvo un error interno).")
    except Exception as e:
        print(f"❌ Error al intentar ejecutar el script: {e}")

if __name__ == "__main__":
    convertir_excel_a_csv()
    ejecutar_siguiente_paso()