from core import CodeGenContext
import parsers

def main():
    print("Inicializando CodeGenContext...")
    ctx = CodeGenContext()
    
    print("\n--- Procesando VARMAP ---")
    parsers.process_varmap(ctx)
    
    print("\n--- Procesando SERVICES ---")
    parsers.process_services(ctx)
    
    print("\n--- Procesando DIALOGS ---")
    parsers.process_dialogs(ctx)
    
    print("\n--- Procesando DECISIONS ---")
    parsers.process_generic_atlas("DECISIONS.csv",
        ctx.decision_types, ctx.decision_atlas, 'decision', parsers.builder_decisions)
    
    print("\n--- Procesando PHRASES ---")
    parsers.process_phrases(ctx)

    print("\n--- Procesando ROOMS ---")
    parsers.process_rooms(ctx)
    
    print("\n--- Procesando TYPES (AUTO) ---")
    parsers.process_auto_types(ctx)

    print("\n--- Procesando ACTION CONDITIONS ---")
    parsers.process_action_conds(ctx)

    print("\n--- Procesando ITEMS ---")
    parsers.process_items(ctx)

    print("\n--- Procesando NAMES ---")
    parsers.process_names(ctx)

    print("\n--- Procesando ANIMATIONS y CARD GAMES ---")
    parsers.process_animations(ctx)

    print("\n--- Procesando SPRITES ---")
    parsers.process_sprites(ctx)

    print("\n--- Procesando SOUNDS ---")
    parsers.process_sounds(ctx)

    print("\n--- Procesando EVENTS ---")
    parsers.process_events(ctx)

    print("\n--- Procesando DETAILS ---")
    parsers.process_details(ctx)

    # Añadir totales de módulos
    print("\n--- Añadiendo Módulos ---")
    for mod in ctx.modules:
        ctx.modules_types.insert_line(1, f"MODULE_{mod},\n")
    ctx.modules_types.insert_line(1, '\nMODULE_TOTAL \n')

    print("\n--- GUARDANDO ARCHIVOS ATG ---")
    ctx.save_all()
    print("¡Generación completada con éxito!")

if __name__ == "__main__":
    main()