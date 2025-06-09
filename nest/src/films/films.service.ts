import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Film } from './film.entity';

@Injectable()
export class FilmsService {
  constructor(
    @InjectRepository(Film)
    private filmsRepository: Repository<Film>,
  ) {}

  findAll(): Promise<Film[]> {
    return this.filmsRepository.find();
  }

  findOne(id: number): Promise<Film | null> {
    return this.filmsRepository.findOne({ where: { filmId: id } });
  }

  async create(film: Partial<Film>): Promise<Film> {
    const newFilm = this.filmsRepository.create(film);
    return this.filmsRepository.save(newFilm);
  }

  async update(id: number, film: Partial<Film>): Promise<Film | null> {
    await this.filmsRepository.update(id, film);
    return this.filmsRepository.findOne({ where: { filmId: id } });
  }

  async remove(id: number): Promise<void> {
    await this.filmsRepository.delete(id);
  }
}