import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ 
    name: 'as', pure: true 
})
export class AsPipe implements PipeTransform {
  transform<T>(input: unknown, baseItem: T | undefined): T {
    return (input as unknown) as T;
  }
}