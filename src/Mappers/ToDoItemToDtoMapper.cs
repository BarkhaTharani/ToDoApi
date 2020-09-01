
using ToDoApi.Data.Dtos;
using ToDoApi.Data.Entities;

namespace ToDoApi.Mappers {
    public class ToDoItemToDtoMapper {
        public static TodoItemDto MapEntity (ToDoItem item) {
            var dto = new  TodoItemDto {
                id = item.id,
                name = item.name,
                isComplete = item.isComplete,
            };

            return dto;
        }

        public static ToDoItem MapDto (TodoItemDto dto) {
            var item = new  ToDoItem {
                name = dto.name,
                isComplete = dto.isComplete,
            };

            return item;
        }
    }
}