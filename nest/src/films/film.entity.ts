import { Entity, PrimaryColumn, Column, UpdateDateColumn } from 'typeorm';

@Entity('film')
export class Film {
  @PrimaryColumn({ name: 'film_id' })
  filmId: number;

  @Column()
  title: string;

  @Column({ type: 'text' })
  description: string;

  @Column({ name: 'release_year', type: 'integer' })
  releaseYear: number;

  @Column({ name: 'language_id', type: 'integer' })
  languageId: number;

  @Column({
    name: 'original_language_id',
    type: 'integer',
    nullable: true,
  })
  originalLanguageId: number | null;

  @Column({ name: 'rental_duration', type: 'integer' })
  rentalDuration: number;

  @Column({ name: 'rental_rate', type: 'numeric' })
  rentalRate: string; // numeric is better handled as string in TypeORM

  @Column({ type: 'integer' })
  length: number;

  @Column({ name: 'replacement_cost', type: 'numeric' })
  replacementCost: string; // numeric as string

  @Column({ type: 'varchar' })
  rating: string;

  @UpdateDateColumn({ name: 'last_update', type: 'timestamp with time zone' })
  lastUpdate: Date;

  @Column({
    name: 'special_features',
    type: 'jsonb',
    nullable: true,
  })
  specialFeatures: string[];

  @Column({
    type: 'tsvector',
    select: false,
    nullable: true,
  })
  fulltext: string;
}
