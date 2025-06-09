import {
  Controller,
  Get,
  Post,
  Body,
  Param,
  Put,
  Delete,
} from '@nestjs/common';
import { FilmsService } from './films.service';
import { Film } from './film.entity';

@Controller('films')
export class FilmsController {
  constructor(private readonly filmsService: FilmsService) {}

  @Get()
  async findAll(): Promise<Film[]> {
    return this.filmsService.findAll();
  }

  @Get(':id')
  async findOne(@Param('id') id: string): Promise<Film | null> {
    return this.filmsService.findOne(+id);
  }

  @Post()
  async create(@Body() film: Film): Promise<Film> {
    return this.filmsService.create(film);
  }

  @Put(':id')
  async update(
    @Param('id') id: string,
    @Body() film: Partial<Film>,
  ): Promise<Film | null> {
    return this.filmsService.update(+id, film);
  }

  @Delete(':id')
  async remove(@Param('id') id: string): Promise<void> {
    return this.filmsService.remove(+id);
  }
}
